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

using BH.oM.Base;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BH.Engine.Reflection
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Set the value of a property with a given name for an object")]
        [Input("obj", "object to set the value for")]
        [Input("propName", "name of the property to set the value of")]
        [Input("value", "new value of the property. \nIf left empty, the value for that property will be cleared \n(enumerables will be emptied, primitives will be set to their default value, and objects will be set to null)")]
        [Output("result", "New object with its property changed to the new value")]
        public static object SetPropertyValue(this object obj, string propName, object value = null)
        {
            object toChange = obj;
            if (propName.Contains("."))
            {
                string[] props = propName.Split('.');
                for (int i = 0; i < props.Length - 1; i++)
                {
                    toChange = toChange.PropertyValue(props[i]);
                    if (toChange == null)
                        break;
                }
                propName = props[props.Length - 1];
            }

            System.Reflection.PropertyInfo prop = toChange.GetType().GetProperty(propName);
            if (prop != null)
            {
                if (!prop.CanWrite)
                {
                    Engine.Reflection.Compute.RecordError("This property doesn't have a public setter so it is not possible to modify it.");
                    return obj;
                }

                Type propType = prop.PropertyType;
                if (value == null)
                {
                    if (propType == typeof(string))
                        value = "";
                    else if (propType.IsValueType || typeof(IEnumerable).IsAssignableFrom(propType))
                        value = Activator.CreateInstance(propType);
                }

                if (value != null)
                {
                    if (value.GetType() != propType && value.GetType().GenericTypeArguments.Length > 0 && propType.GenericTypeArguments.Length > 0)
                    {
                        value = Modify.CastGeneric(value as dynamic, propType.GenericTypeArguments[0]);
                    }
                    if (value.GetType() != propType)
                    {
                        ConstructorInfo constructor = propType.GetConstructor(new Type[] { value.GetType() });
                        if (constructor != null)
                            value = constructor.Invoke(new object[] { value });
                    }
                }
                
                prop.SetValue(toChange, value);
                return obj;
            }
            else 
            {
                SetValue(toChange as dynamic, propName, value);
                return obj;
            }
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static bool SetValue(this IBHoMObject obj, string propName, object value)
        {
            if (obj == null) return false;

            if (value is IFragment)
            {
                obj.Fragments.Add(value as IFragment);
                Compute.RecordWarning("The object does not contain any property with the name " + propName + ". The value is being set as a fragment.");
            }
            else
            {
                obj.CustomData[propName] = value;
                Compute.RecordWarning("The object does not contain any property with the name " + propName + ". The value is being set as custom data.");
            }

            return true;
        }

        /***************************************************/

        private static bool SetValue(this IDictionary dic, string propName, object value)
        {
            dic[propName] = value;
            return true;
        }

        /***************************************************/

        private static bool SetValue<T>(this IEnumerable<T> list, string propName, object value)
        {
            bool success = true;

            foreach (T item in list)
                success &= SetPropertyValue(item, propName, value) != null;

            return success;
        }

        /***************************************************/

        private static bool SetValue(this object obj, string propName, object value)
        {
            Compute.RecordWarning("The objects does not contain any property with the name " + propName + ".");
            return false;
        }

        /***************************************************/
    }
}
