﻿/*
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
using BH.oM.Analytical.Elements;
using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BH.Engine.Analytical
{
    public static partial class Query
    {
        public static Dictionary<Guid, int> DepthDictionary(Dictionary<Guid, List<Guid>> adjacency, Guid startEntity)
        {
            //https://www.geeksforgeeks.org/level-node-tree-source-node-using-bfs/
            // dictionary to store level of each node  
            Dictionary<Guid, int> level = new Dictionary<Guid, int>();
            if (!adjacency.ContainsKey(startEntity))
            {
                Reflection.Compute.RecordError("startEntity provided cannot be found in the adjacency dictionary. Ensure the node exists in the original graph");
                return level;
            }
            // dictionary to store when node has been visited
            Dictionary<Guid, bool> marked = new Dictionary<Guid, bool>();
            // create a queue  
            Queue<Guid> que = new Queue<Guid>();
            // enqueue element x  
            que.Enqueue(startEntity);
            // initialize level of source node to 0  
            level[startEntity] = 0;
            // marked it as visited  
            marked[startEntity] = true;
            // do until queue is empty  
            while (que.Count > 0)
            {
                // dequeue element  
                startEntity = que.Dequeue();
                // traverse neighbors of node x  
                foreach (Guid b in adjacency[startEntity])
                {
                    // b is neighbor of node x  
                    // if b is not marked already  
                    if (!marked.ContainsKey(b))
                    {
                        // enqueue b in queue  
                        que.Enqueue(b);
                        // level of b is level of x + 1  
                        level[b] = level[startEntity] + 1;
                        // mark b  
                        marked[b] = true;
                    }
                }
            }
            return level;
        }
        /***************************************************/
        [Description("Gets the depth dictionary of a Graph<INode> using breadth first search, each key value pair in the resulting dictionary is in the form <Graph<INode> node, depth>")]
        [Input("graph", "The Graph<INode> to extract the depth dictionary from")]
        [Input("starGuid", "The Graph<INode> node from which the depth dictionary is created")]
        public static Dictionary<Guid, int> DepthDictionary(this Graph graph, Guid startEntity)
        {
            Dictionary<Guid, List<Guid>> adjacency = graph.Adjacency();
            return DepthDictionary(adjacency, startEntity);
        }
    }
}