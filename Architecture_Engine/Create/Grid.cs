﻿using BH.oM.Geometry;
using BH.oM.Architecture.Elements;
using System.Collections.Generic;

namespace BH.Engine.Architecture.Elements
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Grid Grid(ICurve curve)
        {
            return new Grid
            {
                Curve = curve
            };
        }

        /***************************************************/

        public static Grid Grid(Point origin, Vector direction)
        {
            Plane plane = new Plane { Origin = origin, Normal = Engine.Geometry.Query.CrossProduct(direction, Vector.ZAxis) };
            Line line = new Line { Start = origin, End = origin + direction * 20 };
            return new Grid { Curve = line };
        }

        /***************************************************/
    }
}
