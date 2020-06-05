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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.Results.Mesh;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Environment
{
    public static partial class Query
    {
        [Description("Filter a collection of Environment MeshResult objects to those which match the given object ID")]
        [Input("results", "A collection of Environment MeshResult objects to filter")]
        [Input("objectID", "The Object ID to filter by")]
        [Output("filteredResults", "The collection of Environment MeshResults which have the provided object ID")]
        public static List<MeshResult> FilterResultsByObjectID(this List<MeshResult> results, IComparable objectID)
        {
            return results.Where(x => x.ObjectId == objectID).ToList();
        }

        [Description("Filter a collection of Environment MeshResult objects to those which match the given result case")]
        [Input("results", "A collection of Environment MeshResult objects to filter")]
        [Input("resutlCase", "The Result Case to filter by")]
        [Output("filteredResults", "The collection of Environment MeshResults which have the provided result case")]
        public static List<MeshResult> FilterResultsByResultCase(this List<MeshResult> results, IComparable resultCase)
        {
            return results.Where(x => x.ResultCase == resultCase).ToList();
        }
    }
}
