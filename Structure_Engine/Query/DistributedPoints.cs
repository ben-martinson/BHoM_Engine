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

using System.Collections.Generic;

using BH.oM.Geometry;
using BH.oM.Structure.Elements;

using BH.oM.Reflection.Attributes;
using BH.oM.Quantities.Attributes;
using System.ComponentModel;

namespace BH.Engine.Structure
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets a list of evenly distributed points along a bar from a given number of dividsions")]
        [Input("bar", "The bar to get division points for")]
        [Input("divisions", "Number of segments to divide the bar into. The number of points returned will be divisions +1.")]
        [Input("startLength", "Optional offset from bar start node to start of divisions.", typeof(Length))]
        [Input("startLength", "Optional offset from bar end node to start of divisions.", typeof(Length))]
        [Output("points", "List of evenly distibuted points along the bar.")]
        public static List<Point> DistributedPoints(this Bar bar, int divisions, double startLength = 0, double endLength = 0)
        {
            if (divisions < 1)
            {
                Reflection.Compute.RecordWarning("Cant handle 0 or negative divisions. Divisions has been set to 1!");
                divisions = 1;
            }

            Point startPos;
            Vector tan;
            if (startLength == 0 && endLength == 0)
            {
                startPos = bar.StartNode.Position;
                tan = bar.Tangent() / (double)divisions;
            }
            else
            {
                double length = bar.Length();
                tan = bar.Tangent() / length;
                startPos = bar.StartNode.Position + tan * startLength;

                tan *= (length - endLength - startLength) / (double)divisions;
            }

            List<Point> pts = new List<Point>();

            for (int i = 0; i <= divisions; i++)
            {
                pts.Add(startPos + tan * i);
            }
            return pts;
        }

        /***************************************************/
    }
}

