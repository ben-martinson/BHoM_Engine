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
        [Description("Clones the IBHoMObjects, computes their hash and stores it in a HashFragment. " +
            "If the object already has a HashFragment, it computes the current one and keeps the old one in the `previousHash` of the HashFragment.")]
        public static Diff AddToDiff(this Diff diff, Diff toAdd)
        {
            Diff newDiff = new Diff(
                diff.AddedObjects.Concat(toAdd.AddedObjects),
                diff.RemovedObjects.Concat(toAdd.RemovedObjects),
                diff.ModifiedObjects.Concat(toAdd.ModifiedObjects),
                diff.DiffConfig,
                diff.ModifiedPropsPerObject.Concat(toAdd.ModifiedPropsPerObject).ToDictionary(x => x.Key, x => x.Value),
                diff.UnchangedObjects.Concat(toAdd.UnchangedObjects)
                );

            return newDiff;
        }
    }
}

