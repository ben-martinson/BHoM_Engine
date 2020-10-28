﻿using BH.Engine.Base;
using BH.oM.Analytical.Elements;
using BH.oM.Analytical.Fragments;
using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.Engine.Analytical
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        [Description("Convert IDependencyFragment assigned to relations")]
        public static List<IRelation> IToRelation(this IDependencyFragment dependency, Guid owner)
        {
            return ToRelation(dependency as dynamic, owner);
        }
        /***************************************************/
        [Description("Convert InputsFragment assigned to relations")]
        public static List<IRelation> ToRelation(this InputsFragment dependency, Guid owner)
        {
            List<IRelation> relations = new List<IRelation>();
            dependency.Inputs.ForEach(input => relations.Add(new Relation() { Source = input, Target = owner }));
            return relations;
        }
        
        [Description("Convert InputOutputFragment assigned to relations")]
        public static List<IRelation> ToRelation(this InputOutputFragment dependency, Guid owner)
        {
            List<IRelation> relations = new List<IRelation>();
            relations.Add(new Relation() { Source = dependency.Input, Target = owner });
            relations.Add(new Relation() { Source = owner, Target = dependency.Output });
            return relations;
        }
        /***************************************************/
        [Description("Convert DependencyFragment assigned to relations")]
        public static List<IRelation> ToRelation(this DependencyFragment dependency, Guid owner)
        {
            List<IRelation> relations = new List<IRelation>();
            relations.Add(new Relation() { Source = dependency.Source, Target = dependency.Target });
            return relations;
        }
        /***************************************************/
        [Description("Convert ProcessDependencyFragment assigned to relations")]
        public static List<IRelation> ToRelation(this ProcessDependencyFragment dependency, Guid owner)
        {
            List<IRelation> relations = new List<IRelation>();
            relations.Add(new ProcessRelation() 
            { 
                Source = dependency.Source, 
                Target = dependency.Target,
                Processes = dependency.Processes
            });
            return relations;
        }
        /***************************************************/
        [Description("Convert SpatialDependencyFragment assigned to relations")]
        public static List<IRelation> ToRelation(this SpatialDependencyFragment dependency, Guid owner)
        {
            List<IRelation> relations = new List<IRelation>();
            relations.Add(new SpatialRelation() 
            { 
                Source = dependency.Source, 
                Target = dependency.Target,
                Curve = dependency.Curve
            });
            return relations;
        }
        /***************************************************/
        /**** Public Methods non IDependency converts   ****/
        /***************************************************/
        [Description("Convert an ILink to a collection of relations")]
        public static List<IRelation> ToRelation<TNode>(this ILink<TNode> link, RelationDirection linkDirection = RelationDirection.Forwards)
            where TNode : INode
        {
            List<IRelation> relations = new List<IRelation>();
            IRelation forward = ToRelation(link);
            switch (linkDirection)
            {
                case RelationDirection.Forwards:
                    relations.Add(forward);
                    break;
                case RelationDirection.Backwards:
                    relations.Add(forward.Reverse());
                    break;
                case RelationDirection.Both:
                    relations.Add(forward); 
                    relations.Add(forward.Reverse());
                    break;
            }
            return relations;
        }
        /***************************************************/
        [Description("Convert a collection of ILinks to a collection of Relations")]
        //UI cannot handle the list of generic inputs yet
        public static List<IRelation> ToRelation<TNode>(this List<ILink<TNode>> links, RelationDirection linkDirection)
            where TNode : INode
        {
            List<IRelation> relations = new List<IRelation>();
            links.ForEach(lnk => lnk.ToRelation(linkDirection));
            return relations;
        }
        /***************************************************/
        [Description("Convert a an ILink to a single forward direction relation")]
        private static IRelation ToRelation<TNode>(this ILink<TNode> link)
            where TNode : INode
        {
            SpatialRelation relation = new SpatialRelation()
            {
                Source = link.StartNode.BHoM_Guid,
                Target = link.EndNode.BHoM_Guid,
                Curve = (ICurve)link.IGeometry()
            };

            Graph subgraph = new Graph();
            subgraph.Entities.Add(link.StartNode.BHoM_Guid, link.StartNode);
            subgraph.Entities.Add(link.EndNode.BHoM_Guid, link.EndNode);
            subgraph.Entities.Add(link.BHoM_Guid, link);
            subgraph.Relations.Add(new Relation() { Source = link.StartNode.BHoM_Guid, Target = link.BHoM_Guid });
            subgraph.Relations.Add(new Relation() { Source = link.BHoM_Guid, Target = link.StartNode.BHoM_Guid });
            relation.Subgraph = subgraph;

            return relation;
        }
        /***************************************************/
        [Description("Extract relations from a collection of IBHoMObjects")]
        public static List<IRelation> ToRelation(this List<IBHoMObject> objs)
        {
            List<IRelation> relations = new List<IRelation>();
            foreach (IBHoMObject obj in objs)
            {
                List<IFragment> dependencyFragments = obj.GetAllFragments(typeof(IDependencyFragment));
                foreach (IDependencyFragment dependency in dependencyFragments)
                    relations.AddRange(dependency.IToRelation(obj.BHoM_Guid));
            }
            return relations;
        }
        /***************************************************/
        /**** Fallback Methods                          ****/
        /***************************************************/

        private static List<IRelation> ToRelation(this IDependencyFragment dependency, Guid owner)
        {
            // Do nothing
            List<IRelation> relations = new List<IRelation>();
            
            return relations;
        }

        /***************************************************/
    }
}
