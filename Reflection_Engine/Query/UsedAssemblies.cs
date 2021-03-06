/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Reflection
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Collect al the assemblies referenced by the input assembly.")]
        [Input("assembly", "Assembly that needs its references collected.")]
        [Input("onlyBHoM", "Only return referenced assemblies that are part of the BHoM.")]
        [Input("goDeep", "Recursively collect all references so that indirect references are also returned.")]
        [Output("assemblies", "List of assemblies referenced by the input assembly.")]
        public static List<Assembly> UsedAssemblies(this Assembly assembly, bool onlyBHoM = false, bool goDeep = false)
        {
            try
            {
                lock (m_GetAssembliesLock)
                {
                    if (m_AllAssemblies == null || m_AllAssemblies.Count == 0)
                        ExtractAllAssemblies();

                    if (goDeep)
                        return DeepDependencies(new List<Assembly> { assembly }, onlyBHoM);
                    else
                    {
                        IEnumerable<AssemblyName> assemblyNames = assembly.GetReferencedAssemblies();
                        Dictionary<string, Assembly> dic = onlyBHoM ? m_BHoMAssemblies : m_AllAssemblies;
                        return assemblyNames.Where(x => dic.ContainsKey(x.FullName)).Select(x => dic[x.FullName]).ToList();
                    }  
                }
            }
            catch (Exception e)
            {
                Compute.RecordWarning("failed to get the assemblies used by " + assembly.FullName + ".\nError: " + e.ToString());
                return new List<Assembly>();
            }
        }

        /***************************************************/

        [Description("Collect al the assemblies referenced by the list of input assemblies.")]
        [Input("assemblyNames", "List of all the assembly namesthat needs that need their references collected.")]
        [Input("onlyBHoM", "Only return referenced assemblies that are part of the BHoM.")]
        [Input("goDeep", "Recursively collect all references so that indirect references are also returned.")]
        [Output("assemblies", "List of assemblies referenced by the input assemblies.")]
        public static List<string> UsedAssemblies(List<string> assemblyNames, bool onlyBHoM = false, bool goDeep = false)
        {
            AllAssemblyList();
            Dictionary<string, Assembly> dic = onlyBHoM ? m_BHoMAssemblies : m_AllAssemblies;
            List<Assembly> assemblies = assemblyNames.Where(x => dic.ContainsKey(x)).Select(x => dic[x]).ToList();

            if (goDeep)
                return DeepDependencies(assemblies, onlyBHoM).Select(x => x.Location).ToList();
            else
                return assemblies.SelectMany(x => x.UsedAssemblies(onlyBHoM)).Select(x => x.Location).Distinct().ToList();
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static List<Assembly> DeepDependencies(List<Assembly> assemblies, bool onlyBHoM = false, List<Assembly> collected = null, int depth = 0)
        {
            if (collected == null)
                collected = assemblies.ToList();

            if (depth > 20)
                return new List<Assembly>();

            Dictionary<string, Assembly> dic = onlyBHoM ? m_BHoMAssemblies : m_AllAssemblies;

            IEnumerable<AssemblyName> assemblyNames = assemblies.SelectMany(x => x.GetReferencedAssemblies())
                .GroupBy(x => x.FullName).Select(x => x.First())
                .ToList();
            
            List<Assembly> dependencies = assemblyNames.Where(x => dic.ContainsKey(x.FullName))
                .Select(x => dic[x.FullName])
                .Except(collected)
                .ToList();

            collected.AddRange(dependencies);

            return dependencies.Concat(DeepDependencies(dependencies, onlyBHoM, collected, depth+1)).ToList();
        }

        /***************************************************/
    }
}


