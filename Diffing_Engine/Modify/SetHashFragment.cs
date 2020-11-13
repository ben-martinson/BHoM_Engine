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

using BH.oM.Base;
using BH.oM.Data.Collections;
using BH.oM.Diffing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using BH.Engine.Serialiser;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.Engine.Base;

namespace BH.Engine.Diffing
{
    public static partial class Modify
    {
        [Description("Clones the IBHoMObjects, computes their hash and stores it in a HashFragment.")]
        public static List<T> SetHashFragment<T>(this IEnumerable<T> objs, DistinctConfig distinctConfig = null) where T : IBHoMObject
        {
            // Clone the current objects to preserve immutability
            List<T> objs_cloned = new List<T>();

            // Set configurations if diffConfig is null
            distinctConfig = distinctConfig == null ? new DistinctConfig() : distinctConfig;

            // Calculate and set the object hashes
            foreach (var obj in objs)
                objs_cloned.Add(SetHashFragment(obj, distinctConfig));

            return objs_cloned;
        }

        [Description("Clones the IBHoMObject, computes their hash and stores it in a HashFragment.")]
        public static T SetHashFragment<T>(T obj, DistinctConfig distinctConfig = null) where T : IBHoMObject
        {
            // Clone the current object to preserve immutability
            T obj_cloned = BH.Engine.Base.Query.DeepClone(obj);

            // Set configurations if diffConfig is null
            distinctConfig = distinctConfig == null ? new DistinctConfig() : distinctConfig;

            // Calculate and set the object hashes
            string hash = obj_cloned.Hash(distinctConfig);
            obj_cloned.Fragments.AddOrReplace(new HashFragment() { Hash = hash });

            return obj_cloned;
        }

        [Description("Clones the IBHoMObject, computes their hash and stores it in a HashFragment.")]
        public static T SetHashFragment<T>(T obj, string hash) where T : IBHoMObject
        {
            // Clone the current object to preserve immutability
            T obj_cloned = BH.Engine.Base.Query.DeepClone(obj);

            obj_cloned.Fragments.AddOrReplace(new HashFragment() { Hash = hash });

            return obj_cloned;
        }
    }
}

