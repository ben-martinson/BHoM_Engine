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
using BH.oM.Structure.Elements;
using BH.oM.Reflection.Attributes;
using BH.Engine.Reflection;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using BH.oM.Geometry;
using System;
using System.Linq;
using System.ComponentModel;

namespace BH.Engine.Analytical
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Determines whether a panel is a square")]
        [Input("panel", "The Panel to check if it is a square")]
        [Output("bool", "True for square panels or false for non-square panels")]
        public static bool IsSquare(this Panel panel)
        {
            List<ICurve> curves = panel.ExternalEdgeCurves();

            if (curves.Count < 3 || !(panel.Openings.Count==0))
                return false;

            try
            {
                curves.Select(x => x.IDiscontinuityPoints());
            }
            catch (NotImplementedException)
            {
                return false;
            }

            HashSet<Vector> vectorSet = new HashSet<Vector>(curves.Select(x => x.IStartDir()));
            HashSet<double> length = new HashSet<double>(vectorSet.Select(x=>x.Length()));

            return vectorSet.Count == 4 && length.Count == 1 ? true : false;
        }

        /***************************************************/

    }

}