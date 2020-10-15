﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using BH.oM.Spatial.ShapeProfiles;
using BH.oM.Geometry;
using System;
using BH.Engine.Reflection;
using BH.oM.Reflection.Attributes;
using BH.Engine.Geometry;
using System.ComponentModel;

namespace BH.Engine.Spatial
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates a single FreeFormProfile from a collection of ICurves. \n" +
                     "Checks if it's a valid profile and attempts to fix;  \n" +
                     " - coplanarity with eachother, by projecting onto the plane of the curve with the biggest area. \n" +
                     " - coplanarity with XYPlane, by rotating the curves to align. \n" +
                     " - curves on the XYPlane, by translating from one controlpoint to the origin. \n" +
                     "Checks if it's a valid profile; If they're closed, Not zero area, curve curve intersections, selfintersections. ")]
        public static FreeFormProfile FreeFormProfile(IEnumerable<ICurve> edges)
        {
            IEnumerable<ICurve> result = edges.ToList();

            List<Point> cPoints = edges.SelectMany(x => x.IControlPoints()).ToList();

            // Any Point not on the XY-Plane
            if (cPoints.Any(x => Math.Abs(x.Z) > Tolerance.Distance))
            {
                // Is Planar
                Plane plane = Geometry.Compute.FitPlane(cPoints);
                bool failedProject = false;
                if (cPoints.Any(x => x.Distance(plane) > Tolerance.Distance))
                {
                    Reflection.Compute.RecordWarning("The Profiles curves are not Planar");
                    try
                    {
                        // Get biggest contributing curve and fit a plane to it
                        plane = Geometry.Compute.IJoin(edges.ToList()).OrderBy(x => x.Area()).Last().ControlPoints().FitPlane();

                        result = edges.Select(x => x.IProject(plane)).ToList();
                        Reflection.Compute.RecordWarning("The Profiles curves have been projected onto a plane fitted through the biggest curve's control points.");
                        cPoints = result.SelectMany(x => x.IControlPoints()).ToList();
                    }
                    catch
                    {
                        failedProject = true;
                    }
                }

                if (!failedProject)
                {
                    // Is Coplanar with XY
                    if (plane.Normal.IsParallel(oM.Geometry.Vector.ZAxis) == 0)
                    {
                        Reflection.Compute.RecordWarning("The Profiles curves are not coplanar with the XY-Plane. Automatic orientation has occured.");

                        plane.Normal = plane.Normal.Z > 0 ? plane.Normal : -plane.Normal;
                        double rad = plane.Normal.Angle(oM.Geometry.Vector.ZAxis);

                        Line axis = plane.PlaneIntersection(oM.Geometry.Plane.XY);
                        result = result.Select(x => x.IRotate(axis.Start, axis.TangentAtParameter(0), rad)).ToList();
                        cPoints = result.SelectMany(x => x.IControlPoints()).ToList();
                    }

                    // Is on XY
                    if (cPoints.FirstOrDefault().Distance(oM.Geometry.Plane.XY) > Tolerance.Distance)
                    {
                        Reflection.Compute.RecordWarning("The Profiles curves are not on the XY-Plane. Automatic translation has occured.");
                        Point p = cPoints.FirstOrDefault();
                        Vector v = new oM.Geometry.Vector() { X = p.X, Y = p.Y, Z = p.Z };

                        result = result.Select(x => x.ITranslate(-v)).ToList();
                    }
                }
            }

            if (!result.Any(x => x.ISubParts().Any(y => y is NurbsCurve)))
            {
                // Join the curves
                List<PolyCurve> joinedCurves = Geometry.Compute.IJoin(result.ToList()).ToList();

                // Is Closed
                if (joinedCurves.Any(x => !x.IsClosed()))
                    Reflection.Compute.RecordWarning("The Profiles curves does not form closed curves");

                // Has non-zero area
                if (joinedCurves.Any(x => x.IArea() < Tolerance.Distance))
                    Reflection.Compute.RecordWarning("One or more of the profile curves have close to zero area.");

                if (!joinedCurves.Any(x => x.ISubParts().Any(y => y is Ellipse)))
                {
                    // Check curve curve Intersections
                    bool intersects = false;
                    for (int i = 0; i < joinedCurves.Count - 1; i++)
                    {
                        for (int j = i + 1; j < joinedCurves.Count; j++)
                        {
                            if (joinedCurves[i].CurveIntersections(joinedCurves[j]).Count > 0)
                            {
                                intersects = true;
                                break;
                            }
                        }
                        if (intersects)
                            break;
                    }
                    if (intersects)
                        Engine.Reflection.Compute.RecordWarning("The Profiles curves are intersecting eachother.");
                }
                // Is SelfInteersecting
                if (joinedCurves.Any(x => x.IsSelfIntersecting()))
                    Engine.Reflection.Compute.RecordWarning("One or more of the Profiles curves is intersecting itself.");

            }

            return new FreeFormProfile(result);
        }

        /***************************************************/
        
    }
}
