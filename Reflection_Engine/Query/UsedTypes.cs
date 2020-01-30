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
using System.Reflection;
using System.Linq;
using Mono.Cecil;
using Mono.Reflection;

namespace BH.Engine.Reflection
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<Type> UsedTypes(this MethodBase method, bool onlyBHoM = false)
        {
            try
            {
                IEnumerable<Type> varTypes = new List<Type>();
                MethodBody body = method.GetMethodBody();
                if (body != null)
                    varTypes = method.GetMethodBody().LocalVariables.Select(x => x.LocalType);

                IEnumerable<Type> methodTypes = method.UsedMethods(onlyBHoM).Select(x => x.DeclaringType);
                IEnumerable<Type> paramTypes = method.GetParameters().Select(x => x.ParameterType);
                IEnumerable<Type> types = methodTypes.Union(paramTypes).Union(varTypes).Distinct();

                if (onlyBHoM)
                    return types.Where(x => x.Namespace.StartsWith("BH.")).ToList();
                else
                    return types.ToList();
            }
            catch (Exception e)
            {
                Compute.RecordError(method.ToString() + " failed t oextract its types.\nError: " + e.ToString());
                return new List<Type>();
            }
        }

        /***************************************************/

        public static List<Type> UsedTypes(this Type type, bool onlyBHoM = false, bool inheritedTypes = true)
        {
            BindingFlags bindingAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance;

            IEnumerable<Type> fieldTypes = type.GetFields(bindingAll).Select(x => x.FieldType).Distinct();
            IEnumerable<Type> propertyTypes = type.GetProperties(bindingAll).Select(x => x.PropertyType).Distinct();
            IEnumerable<Type> methodTypes = type.GetMethods(bindingAll).SelectMany(x => x.UsedTypes()).Distinct();

            List<Type> types = fieldTypes.Union(propertyTypes).Union(methodTypes)
                .Distinct()
                .Where(x => !x.IsAutoGenerated())
                .ToList();

            if (inheritedTypes)
            {
                types = types.Union(type.GetInterfaces()).Distinct().ToList();
                if (type.BaseType != typeof(object))
                    types.Add(type.BaseType);
            }

            if (onlyBHoM)
                return types.Where(x => x.Namespace.StartsWith("BH.")).ToList();
            else
                return types;
        }

        /***************************************************/
    }
}

