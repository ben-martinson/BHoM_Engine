/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using BH.oM.Physical.Elements;
using BH.oM.Geometry;
using BH.oM.Physical.FramingProperties;
using BH.Engine.Geometry;

namespace BH.Engine.Physical
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the centreline geometry from the framing element")]
        [Description("Gets the 3d geometry from the framing element")]
        [Output("3d", "The composite geometry representing the framing element")]
        public static IGeometry Geometry3D(this Beam beam)
        {
            Line line = beam.Location as Line;

            if (line == null)
            {
                BH.Engine.Reflection.Compute.RecordError($"Geometry3D for {nameof(Beam)} currently works only if it has its {nameof(Beam.Location)} defined as a {nameof(Line)}.");
                return null;
            }

            Vector extrusionVec = BH.Engine.Geometry.Create.Vector(line.Start, line.End);
            Vector normal = line.ElementNormal(0);
            IFramingElementProperty prop = beam.Property;

            ConstantFramingProperty constantFramingProperty = prop as ConstantFramingProperty;
            if (constantFramingProperty == null)
            {
                BH.Engine.Reflection.Compute.RecordError($"Geometry3D for {nameof(Beam)} currently works only if its {nameof(Beam.Property)} is of type {nameof(ConstantFramingProperty)}.");
                return null;
            }

            List<ICurve> profileToExtrude = constantFramingProperty.Profile.Edges.ToList();

            if (profileToExtrude == null || !profileToExtrude.Any())
            {
                BH.Engine.Reflection.Compute.RecordError($"Geometry3D error: could not gather the profile curve to be extruded for this {nameof(Beam)}.");
                return null;
            }

            TransformMatrix totalTransform = SectionTranformation(line.Start, extrusionVec.Normalise(), normal);

            return Extrude(profileToExtrude, totalTransform, extrusionVec);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static CompositeGeometry Extrude(List<ICurve> sectionCurves, TransformMatrix matrix, Vector tangent)
        {
            List<IGeometry> extrusions = new List<IGeometry>();

            List<PolyCurve> joined = BH.Engine.Geometry.Compute.IJoin(sectionCurves);

            for (int i = 0; i < joined.Count; i++)
            {
                ICurve curve = joined[i];
                curve = BH.Engine.Geometry.Modify.ITransform(curve, matrix);
                extrusions.Add(new Extrusion() { Curve = curve, Direction = tangent });
            }

            return new CompositeGeometry() { Elements = extrusions };
        }


        /***************************************************/

        [Description("Constructs the transformation matrix needed to move the profile of an element from the default drawing position (around the global origin) to the start of the Element, aligned with its tangent.")]
        private static TransformMatrix SectionTranformation(Point startPoint, Vector tangent, Vector normal)
        {
            Vector trans = startPoint - Point.Origin;

            Vector gX = Vector.XAxis;
            Vector gY = Vector.YAxis;
            Vector gZ = Vector.ZAxis;

            Vector lX = tangent;
            Vector lZ = normal;
            Vector lY = lZ.CrossProduct(lX);

            TransformMatrix localToGlobal = new TransformMatrix();

            localToGlobal.Matrix[0, 0] = gX.DotProduct(lX);
            localToGlobal.Matrix[0, 1] = gX.DotProduct(lY);
            localToGlobal.Matrix[0, 2] = gX.DotProduct(lZ);

            localToGlobal.Matrix[1, 0] = gY.DotProduct(lX);
            localToGlobal.Matrix[1, 1] = gY.DotProduct(lY);
            localToGlobal.Matrix[1, 2] = gY.DotProduct(lZ);

            localToGlobal.Matrix[2, 0] = gZ.DotProduct(lX);
            localToGlobal.Matrix[2, 1] = gZ.DotProduct(lY);
            localToGlobal.Matrix[2, 2] = gZ.DotProduct(lZ);
            localToGlobal.Matrix[3, 3] = 1;

            return Engine.Geometry.Create.TranslationMatrix(trans) * localToGlobal * GetGlobalToSectionAxes();
        }

        /***************************************************/

        [Description("Returns the transformation matrix that transforms from Global to the Section Axes (around the global origin).")]
        private static TransformMatrix GetGlobalToSectionAxes()
        {
            Vector gX = Vector.XAxis;
            Vector gY = Vector.YAxis;
            Vector gZ = Vector.ZAxis;

            //Global system vectors, Sections are drawn in global XY plane with y relating to the normal
            Vector lX = Vector.ZAxis;
            Vector lY = Vector.XAxis;
            Vector lZ = Vector.YAxis;

            TransformMatrix transform = new TransformMatrix();

            transform.Matrix[0, 0] = lX.DotProduct(gX);
            transform.Matrix[0, 1] = lX.DotProduct(gY);
            transform.Matrix[0, 2] = lX.DotProduct(gZ);

            transform.Matrix[1, 0] = lY.DotProduct(gX);
            transform.Matrix[1, 1] = lY.DotProduct(gY);
            transform.Matrix[1, 2] = lY.DotProduct(gZ);

            transform.Matrix[2, 0] = lZ.DotProduct(gX);
            transform.Matrix[2, 1] = lZ.DotProduct(gY);
            transform.Matrix[2, 2] = lZ.DotProduct(gZ);

            transform.Matrix[3, 3] = 1;

            return transform;
        }
    }
}

