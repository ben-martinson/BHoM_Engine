/*
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

using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.Geometry
{
    public static partial class Query
    {
        /***************************************************/
        /*** Public Methods -One dimentional Element Normal*/
        /***************************************************/

        public static Vector Normal(this ICurve curve, double orientationAngle)
        {
            if (curve.IIsLinear())
            {
                Point p1 = curve.IStartPoint();
                Point p2 = curve.IEndPoint();

                Vector tan = (p2 - p1).Normalise();
                Vector normal;

                if (!IsVertical(p1, p2))
                {
                    normal = Vector.ZAxis;
                    normal = (normal - tan.DotProduct(normal) * tan).Normalise();
                }
                else
                {
                    Vector locY = Vector.YAxis;
                    locY = (locY - tan.DotProduct(locY) * tan).Normalise();
                    normal = tan.CrossProduct(locY);
                }

                return normal.Rotate(orientationAngle, tan);
            }
            else if (curve.IIsPlanar())
            {
                Engine.Reflection.Compute.RecordError("The normal for non-linear elements is not implemented");
                return null;
                //Vector tan = curve.IStartDir();   // Is this how we should define it?
                //Vector tan = framingElement.Location.IEndPoint() - framingElement.Location.IStartPoint();
                //return curve.IFitPlane().Normal.Rotate(orientationAngle, tan);    // Warning: The normal can flip by moving some control points
            }
            else
            {
                Engine.Reflection.Compute.RecordError("The normal for non-planar elements is not implemented");
                return null;
            }

        }
        
        /***************************************************/

    }
}

