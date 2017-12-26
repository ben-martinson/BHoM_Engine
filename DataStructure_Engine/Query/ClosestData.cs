﻿using BH.Engine.Geometry;
using BH.oM.DataStructure;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.DataStructure
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static LocalData<T> ClosestData<T>(this PointMatrix<T> matrix, Point refPt, double maxDist)
        {
            List<LocalData<T>> closePts = matrix.CloseToPoint(refPt, maxDist);

            return closePts.OrderBy(x => x.Position.Distance(refPt)).FirstOrDefault();
        }

        /***************************************************/
    }
}
