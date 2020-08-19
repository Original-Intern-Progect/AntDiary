using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace AntDiary{
    public class Node : IPathNode{

        public int Uid{
            get;
        }

        public Vector2 WorldPosition { get; }
        public Vector2 Pos => WorldPosition;
        
        public IEnumerable<IPathEdge> Edges => edges;
        private List<IPathEdge> edges = new List<IPathEdge>();

        //private Dictionary<Node, PathParam> _connectedNodes;

        //public ReadOnlyDictionary<Node, PathParam> ConnectedNodes => new ReadOnlyDictionary<Node, PathParam>(_connectedNodes);

        internal Node(int uid, Vector2 pos){
            Uid = uid;
            WorldPosition = pos;
            //_connectedNodes = new Dictionary<Node, PathParam>();
        }

        internal void Connect(Node node, PathParam pathParam){
            // if(_connectedNodes.ContainsKey(node)){
            //     _connectedNodes.
            // }
            //_connectedNodes[node] = pathParam;
            // _connectedNodes.Add(node, pathParam);
            
            edges.Add(new NodeEdge(this, node, pathParam.GetCost));
        }

        internal void Disconnect(Node node){
            //_connectedNodes.Remove(node);

            var edgesToRemove = edges.Where(e => e.A == node || e.B == node).ToList();

            foreach (var e in edgesToRemove)
            {
                edges.Remove(e);
            }
        }
        
        


        public override string ToString(){
            
            String connStr = "";
            foreach(var edge in Edges){
                connStr += $"Edge({edge.Cost}), ";
            }
            return $"{Uid}: {WorldPosition.ToString()}   connection:[{connStr}]";
        }
    }

    public class NodeEdge : IPathEdge
    {
        public NodeEdge(IPathNode a, IPathNode b, float cost)
        {
            A = a;
            B = b;
            Cost = cost;
        }

        public IPathNode A { get; }
        public IPathNode B { get; }
        public float Cost { get; }
    }
}
