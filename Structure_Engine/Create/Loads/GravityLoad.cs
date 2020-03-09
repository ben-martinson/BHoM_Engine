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

using BH.oM.Geometry;
using BH.oM.Structure.Loads;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Structure
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates a gravity load to be applied to area elements such as Panels and FEMeshes as well as Bar elements.")]
        [InputFromProperty("loadcase")]
        [InputFromProperty("direction")]
        [Input("objects", "The collection of elements the load should be applied to.")]
        [Input("name", "The name of the created load.")]
        [Output("gravLoad", "The created GravityLoad.")]
        public static GravityLoad GravityLoad(Loadcase loadcase, Vector direction, IEnumerable<IBHoMObject> objects, string name = "")
        {
            return new GravityLoad
            {
                Loadcase = loadcase,
                GravityDirection = direction,
                Objects = new BHoMGroup<BHoMObject>() { Elements = objects.Cast<BHoMObject>().ToList() },
                Name = name
            };
        }

        /***************************************************/
        /**** Public Methods - Deprecated               ****/
        /***************************************************/

        [Deprecated("3.1", "Replaced with autogenerated Property assignement method.")]
        public static GravityLoad GravityLoad(Loadcase loadcase, Vector direction, BHoMGroup<BHoMObject> group, string name = "")
        {
            return new GravityLoad
            {
                Loadcase = loadcase,
                GravityDirection = direction,
                Objects = group,
                Name = name
            };
        }

        /***************************************************/

    }
}

