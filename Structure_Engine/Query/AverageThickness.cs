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

using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Reflection.Attributes;
using BH.oM.Quantities.Attributes;
using System.ComponentModel;
using BH.oM.Structure.SurfaceProperties;
using System;

namespace BH.Engine.Structure
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the average thickness of the property as if it was applied to an infinite plane.")]
        [Input("property", "The property to evaluate the average thickness of.")]
        [Output("averageThickness", "the average thickness of the property as if it was applied to an infinite plane.", typeof(Length))]
        public static double AverageThickness(ConstantThickness property)
        {
            return property.Thickness;
        }

        /***************************************************/

        [Description("Gets the average thickness of the property as if it was applied to an infinite plane.")]
        [Input("property", "The property to evaluate the average thickness of.")]
        [Output("averageThickness", "the average thickness of the property as if it was applied to an infinite plane.", typeof(Length))]
        public static double AverageThickness(Ribbed property)
        {
            if (property.StemWidth == 0 || property.Spacing < property.StemWidth ||
                property.TotalDepth < property.Thickness)
            {
                Reflection.Compute.RecordError("Invalid Ribbed slab, returning null.");
                return double.NaN;
            }
            double avrageThicknessLower = (property.TotalDepth - property.Thickness) * (property.StemWidth / property.Spacing);
            return property.Thickness + avrageThicknessLower;
        }

        /***************************************************/

        [Description("Gets the average thickness of the property as if it was applied to an infinite plane.")]
        [Input("property", "The property to evaluate the average thickness of.")]
        [Output("averageThickness", "the average thickness of the property as if it was applied to an infinite plane.", typeof(Length))]
        public static double AverageThickness(Waffle property)
        {
            if (property.StemWidthX == 0 || property.SpacingX < property.StemWidthX ||
                property.StemWidthY == 0 || property.SpacingY < property.StemWidthY ||
                property.TotalDepthX < property.Thickness || property.TotalDepthY < property.Thickness)
            {
                Reflection.Compute.RecordError("Invalid Waffle slab, returning null.");
                return double.NaN;
            }

            double avrageThicknessLowerX = (property.TotalDepthX - property.Thickness) * (property.StemWidthX / property.SpacingX);
            double avrageThicknessLowerY = (property.TotalDepthY - property.Thickness) * (property.StemWidthY / property.SpacingY);

            double theExtraVolume = Math.Min(property.TotalDepthX - property.Thickness, property.TotalDepthY - property.Thickness) *
                                    (property.StemWidthX * property.StemWidthY) / (property.SpacingX * property.SpacingY);

            return property.Thickness + avrageThicknessLowerX + avrageThicknessLowerY - theExtraVolume;
        }

        /***************************************************/

        [Description("Gets the average thickness of the property as if it was applied to an infinite plane.")]
        [Input("property", "The property to evaluate the average thickness of.")]
        [Output("averageThickness", "the average thickness of the property as if it was applied to an infinite plane.", typeof(Length))]
        public static double AverageThickness(LoadingPanelProperty property)
        {
            Reflection.Compute.RecordWarning("Structural IAreaElements are defined without volume.");
            return 0;
        }
        

        /***************************************************/
        /**** Public Methods - Interfaces               ****/
        /***************************************************/
        
        [Description("Gets the average thickness of the property as if it was applied to an infinite plane.")]
        [Input("property", "The property to evaluate the average thickness of.")]
        [Output("averageThickness", "the average thickness of the property as if it was applied to an infinite plane.", typeof(Length))]
        public static double IAverageThickness(this ISurfaceProperty property)
        {
            return AverageThickness(property as dynamic);
        }


        /***************************************************/
        /**** Private Methods - Fallbacks               ****/
        /***************************************************/

        private static double AverageThickness(this ISurfaceProperty property)
        {
            Reflection.Compute.RecordError(property.GetType().Name + " does not have an implementation for AverageThickness. Returning NaN.");
            return double.NaN;
        }

        /***************************************************/

    }

}


