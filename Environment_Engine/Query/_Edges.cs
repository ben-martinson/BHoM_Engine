/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using System.Collections.Generic;

using System.Linq;
using BH.oM.Environment.Elements;

using BH.Engine.Geometry;
using BH.oM.Geometry;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Environment
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("BH.Engine.Environment.Query.Azimuth => Returns the azimuth of a given environmental object")]
        [Input("environmentObject", "Any object implementing the IEnvironmentObject interface that can have its azimuth queried")]
        [Input("referenceVector", "The reference vector for querying the azimuth from the object")]
        [Output("The azimuth of the Environment Object")]
        public static List<Edge> Edges(this Panel element)
        {
            return element.PanelCurve.ISubParts() as List<Line>;
        }

        public static List<Line> Edges(this List<BuildingElement> space)
        {
            List<Line> parts = new List<Line>();

            foreach (BuildingElement be in space)
                parts.AddRange(be.Edges());

            return parts;
        }

        public static bool EdgeIntersects(this Line edge, List<Line> possibleIntersections)
        {
            return possibleIntersections.Where(x => x.BooleanIntersection(edge) != null).ToList().Count > 0;
        }

        public static bool EdgeIntersects(this List<Line> edges, List<Line> possibleIntersections)
        {
            foreach(Line l in edges)
            {
                if (l.EdgeIntersects(possibleIntersections)) return true;
            }

            return false;
        }

        public static List<Line> UnconnectedEdges(this BuildingElement element, List<BuildingElement> space)
        {
            List<Line> edges = element.Edges();

            List<Line> unconnected = new List<Line>();

            List<Line> allEdges = space.Edges();

            foreach(Line l in edges)
            {
                if (allEdges.Where(x => x.BooleanIntersection(l) != null).ToList().Count < 2)
                    unconnected.Add(l);
            }

            return unconnected;
        }
    }
}
