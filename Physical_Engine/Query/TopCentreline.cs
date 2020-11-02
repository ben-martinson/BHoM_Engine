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

using BH.Engine.Geometry;
using BH.Engine.Reflection;
using BH.Engine.Spatial;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Physical.FramingProperties;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Physical
{
    public static partial class Query
    {
        [Description("Returns the top centreline of an IFramingElement.")]
        [Input("element", "The IFramingElement to query the top centreline of.")]
        [Output("curve", "The top centreline of the IFramingElement.")]

        public static ICurve TopCentreline(this BH.oM.Physical.Elements.IFramingElement element)
        {
            ICurve location = element.Location;

            Vector normal;

            normal = BH.Engine.Physical.Query.Normal(element);

            if (normal == null)
            {
                Engine.Reflection.Compute.RecordError("Can only extract top centrelines from a linear IFramingElement.");
                return null;
            }

            double height = 0;

            object heightProperty = element.PropertyValue("Property.Profile.Height");

            if (heightProperty is IConvertible)
            {
                height = ((IConvertible)heightProperty).ToDouble(null);

                ICurve topCentreline = location.ITranslate(normal * 0.5 * height);

                return topCentreline;
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Was not able to either extract height property or convert into double.");
                return null;
            }

        }

    }
}
