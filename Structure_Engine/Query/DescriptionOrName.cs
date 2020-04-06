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

using BH.oM.Geometry.ShapeProfiles;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Structure.SurfaceProperties;

using BH.oM.Reflection.Attributes;
using BH.oM.Quantities.Attributes;
using System.ComponentModel;

namespace BH.Engine.Structure
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the name from a IProperty. If null or empty, a default description name is provided instead.")]
        [Input("property", "The IProperty go get the name or default description from.")]
        [Output("name", "The name or autogenerated description for the IProperty.")]
        public static string DescriptionOrName(this IProperty property)
        {
            string name = "";
            if (property is IBHoMObject)
            {
                name = (property as IBHoMObject).Name;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return IDescription(property);
            }
            else
            {
                return name;
            }
        }

        /***************************************************/

        [Deprecated("3.2", "To be deleted as can be replaced with more generic method accepting IProperty")]
        [Description("Gets the name from a section. If null or empty, a default description name is provided instead.")]
        [Input("section","The section go get the name or default description from.")]
        [Output("name","The name or autogenerated description for the section.")]
        public static string DescriptionOrName(this ISectionProperty section)
        {
            if (string.IsNullOrWhiteSpace(section.Name))
            {                
                return IDescription(section);
            }
            else
            {
                return section.Name;
            }
        }

        /***************************************************/

        [Deprecated("3.2", "To be deleted as can be replaced with more generic method accepting IProperty")]
        [Description("Gets the name from a property. If null or empty, a default description name is provided instead.")]
        [Input("property", "The property go get the name or default description from.")]
        [Output("name", "The name or autogenerated description for the property.")]
        public static string DescriptionOrName(this ISurfaceProperty property)
        {
            if (string.IsNullOrWhiteSpace(property.Name))
            {
                return IDescription(property);                
            }
            else
            {
                return property.Name;
            }
        }

        /***************************************************/

        [Description("Gets the name from a profile. If null or empty, a default description name is provided instead.")]
        [Input("profile", "The profile go get the name or default description from.")]
        [Output("name", "The name or autogenerated description for the profile.")]
        public static string DescriptionOrName(this IProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.Name))
            {
                return IDescription(profile);
            }
            else
            {
                return profile.Name;
            }
        }

        /***************************************************/
    }
}

