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

using BH.oM.Dimensional;
using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using BH.oM.Spatial.ShapeProfiles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.Spatial
{
    public static partial class Query
    {
        /******************************************/
        /****            IProfile              ****/
        /******************************************/

        [Description("Returns the thickness of a boxProfile.")]
        [Input("boxProfile", "The boxProfile to query.")]
        [Output("thickness", "Thickness of the boxProfile.")]
        public static double Thickness(this BoxProfile boxProfile)
        {
            return boxProfile.Thickness;
        }

        /******************************************/

        [Description("Returns the thickness of a tubeProfile.")]
        [Input("tubeProfile", "The tubeProfile to query.")]
        [Output("thickness", "Thickness of the tubeProfile.")]
        public static double Thickness(this TubeProfile tubeProfile)
        {
            return tubeProfile.Thickness;
        }

        /******************************************/

        [Description("Returns the thickness of a ShapeProfile.")]
        [Input("profile", "The ShapeProfile to query.")]
        [Output("thickness", "Thickness of the ShapeProfile.")]
        public static double Thickness(this IProfile profile)
        {
            return Thickness(profile as dynamic);
        }

        /******************************************/
    }
}

