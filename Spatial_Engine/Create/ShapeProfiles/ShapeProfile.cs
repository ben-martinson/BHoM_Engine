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

using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using BH.oM.Spatial.ShapeProfiles;
using BH.oM.Geometry;
using System;
using BH.Engine.Reflection;
using BH.oM.Reflection.Attributes;
using BH.Engine.Geometry;
using System.ComponentModel;

namespace BH.Engine.Spatial
{
    public static partial class Create
    {

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static List<ICurve> MirrorAboutLocalY(this List<ICurve> curves)
        {
            Plane plane = oM.Geometry.Plane.XZ;
            return curves.Select(x => x.IMirror(plane)).ToList();
        }

        /***************************************************/

        private static List<ICurve> MirrorAboutLocalZ(this List<ICurve> curves)
        {
            Plane plane = oM.Geometry.Plane.YZ;
            return curves.Select(x => x.IMirror(plane)).ToList();
        }

        /***************************************************/

        private static void InvalidRatioError(string first, string second)
        {
            Engine.Reflection.Compute.RecordError("The ratio of the " + first + " in relation to the " + second + " makes section inconceivable");
        }

        /***************************************************/

    }
}
