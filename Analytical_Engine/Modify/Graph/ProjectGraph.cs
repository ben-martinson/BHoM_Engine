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

using BH.Engine.Base;
using BH.Engine.Spatial;
using BH.oM.Analytical.Elements;
using BH.oM.Analytical.Fragments;
using BH.oM.Base;
using BH.oM.Dimensional;
using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Analytical
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Returns a Graph projection defined by the projection provided.")]
        [Input("graph", "The Graph to query.")]
        [Input("projection", "The required IView.")]
        [Output("graph", "The projection of the original Graph.")]
        public static Graph IProjectGraph(this Graph graph, IProjection projection)
        {
            Graph graphView = ProjectGraph(graph, projection as dynamic);
            //ensure we have curves on relations
            //IRelationCurves(graphView, projection as dynamic);
            return graphView;
        }
        /***************************************************/

        [Description("Returns a Graph projection that contains only geometric entities. Spatial entities are those implementing IElement0D.")]
        [Input("graph", "The Graph to query.")]
        [Input("projection", "The SpatialView.")]
        [Output("graph", "The spatial Graph.")]
        private static Graph ProjectGraph(this Graph graph, GeometricProjection projection)
        {
            Graph geometricGraph = graph.DeepClone();
            foreach (IBHoMObject entity in geometricGraph.Entities.Values.ToList())
            {
                if (!typeof(IElement0D).IsAssignableFrom(entity.GetType()))
                    geometricGraph.RemoveEntity(entity.BHoM_Guid);
            }

            return geometricGraph;
        }

        /***************************************************/

        [Description("Returns a Graph projection that contains only spatial entities. Spatial entities are those implementing IElement0D.")]
        [Input("graph", "The Graph to query.")]
        [Input("projection", "The SpatialView.")]
        [Output("graph", "The spatial Graph.")]
        private static Graph ProjectGraph(this Graph graph, SpatialProjection projection)
        {
            Graph spatialGraph = graph.DeepClone();
            //set representation based on projection
            
            return spatialGraph;
        }

        /***************************************************/

        [Description("Returns a process projection of the Graph.")]
        [Input("graph", "The Graph to query.")]
        [Input("projection", "The ProcessView.")]
        [Output("graph", "The process Graph.")]

        private static Graph ProjectGraph(this Graph graph,  GraphicalProjection projection)
        {
            Graph processGraph = graph.DeepClone();
            foreach (IBHoMObject entity in processGraph.Entities.Values.ToList())
            {
                ProjectionFragment projectionFragment = entity.FindFragment<ProjectionFragment>();
                //if no process projection fragment, remove entity
                if (projectionFragment == null)
                    processGraph.RemoveEntity(entity.BHoM_Guid);
                else
                {
                    //if all group names are in the ignore list remove entity
                    //int ignored = 0;
                    //foreach (string group in projectionFragment.GroupNames)
                    //{
                    //    if (projection.ViewConfig.GroupsToIgnore.Contains(group))
                    //        ignored++;
                    //}
                    //if (ignored == projectionFragment.GroupNames.Where(s => !string.IsNullOrWhiteSpace(s)).ToList().Count())
                    //    processGraph.RemoveEntity(entity.BHoM_Guid);
                }
            }
            //processGraph.ILayout(projection);
            return processGraph;
        }

        /***************************************************/
        /**** Fallback Methods                          ****/
        /***************************************************/

        private static Graph ProjectGraph(this Graph graph, IProjection projection)
        {
            Reflection.Compute.RecordError("IView provided does not have a corresponding GraphView method implemented.");
            return new Graph();
        }

        /***************************************************/
    }
}
