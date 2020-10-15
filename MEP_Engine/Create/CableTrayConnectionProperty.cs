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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using BH.oM.Reflection.Attributes;
using BH.oM.MEP.ConnectionProperties;
using BH.oM.MEP.Elements;

namespace BH.Engine.MEP
{
    public static partial class Create
    {
        /***************************************************/
        /****               Public Methods              ****/
        /***************************************************/

        [Description("Returns a cable tray connection property")]
        [Input("startNode", "The point at which the Connector object begins.")]
        [Input("endNode", "The point at which the Connector object ends.")]
        [Input("isStartConnected", "Whether the start point of the Cable Tray is connected to another segment or not.")]
        [Input("isEndConnected", "Whether the end point of the Cable Tray is connected to another segment or not.")]
        [Output("cableTrayConnectionProperty", "A cable tray connection property")]
        public static CableTrayConnectionProperty CableTrayConnectionProperty(Node startNode, Node endNode, bool isStartConnected, bool isEndConnected)
        {
            return new CableTrayConnectionProperty
            {
                StartNode = startNode,
                EndNode = endNode,
                IsStartConnected = isStartConnected,
                IsEndConnected = isEndConnected,      
            };
        }
    }
}

