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

using BH.oM.Dimensional;
using BH.oM.Geometry;
using BH.oM.Analytical.Elements;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.Engine.Base;


namespace BH.Engine.Analytical
{
    public static partial class Modify
    {
        /***************************************************/
        /****               Public Methods              ****/
        /***************************************************/

        [PreviousVersion("4.0", "BH.Engine.Structure.Modify.SetInternalElements2D(BH.oM.Structure.Elements.Panel, System.Collections.Generic.List<BH.oM.Dimensional.IElement2D>)")]
        [Description("Sets internal IElement2Ds of a IPanel, i.e. sets the Openings of an IPanel. Method required for all IElement2Ds.")]
        [Input("panel", "The IPanel to update.")]
        [Input("openings", "The internal IElement2Ds to set. For an IPanel this should be a list of Openings matching the type of the IPanel.")]
        [Output("panel", "The IPanel with updated Openings.")]
        public static IPanel<TEdge, TOpening> SetInternalElements2D<TEdge, TOpening>(this IPanel<TEdge, TOpening> panel, List<IElement2D> openings)
    where TEdge : IEdge
    where TOpening : IOpening<TEdge>
        {
            IPanel<TEdge, TOpening> pp = panel.ShallowClone();
            pp.Openings = new List<TOpening>(openings.Cast<TOpening>().ToList());
            return pp;
        }

        /***************************************************/
    }
}

