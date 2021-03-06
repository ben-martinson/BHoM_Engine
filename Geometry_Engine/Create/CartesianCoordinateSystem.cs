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

using BH.oM.Geometry;
using BH.oM.Geometry.CoordinateSystem;
using System;
using System.ComponentModel;

namespace BH.Engine.Geometry
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates a Cartesian CoordinateSystem. x and y will be unitised. If x and why are non-orthogonal, y will be made orthogonal to x, while x will be kept")]
        public static Cartesian CartesianCoordinateSystem(Point origin, Vector x, Vector y)
        {

            x = x.Normalise();
            y = y.Normalise();

            double dot = x.DotProduct(y);

            if (Math.Abs(1 - dot) < Tolerance.Angle)
                throw new ArgumentException("Can not create coordinate system from parallell vectors");

            Vector z = x.CrossProduct(y);
            z.Normalise();

            if (Math.Abs(dot) > Tolerance.Angle)
            {
                Reflection.Compute.RecordWarning("x and y are not orthogonal. y will be made othogonal to x and z");
                y = z.CrossProduct(x).Normalise();
            }

            return new Cartesian(origin, x, y, z);
        }


        /***************************************************/
        /**** Random Geometry                           ****/
        /***************************************************/

        public static Cartesian RandomCartesianCoordinateSystem(int seed = -1, BoundingBox box = null)
        {
            if (seed == -1)
                seed = m_Random.Next();
            Random rnd = new Random(seed);
            return RandomCartesianCoordinateSystem(rnd, box);
        }

        /***************************************************/

        public static Cartesian RandomCartesianCoordinateSystem(Random rnd, BoundingBox box = null)
        {
            Vector x = RandomVector(rnd, box);
            Vector y = RandomVector(rnd, box);

            if (x.IsParallel(y) != 0)
                return RandomCartesianCoordinateSystem(rnd, box);

            Point orgin = RandomPoint(rnd, box);

            return CartesianCoordinateSystem(orgin, x, y);
        }

        /***************************************************/
    }
}


