using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// NestElementsにおけるパスのノードを示す。
    /// </summary>
    public abstract class NestPathNode : IPathNode
    {
        public NestElement Host { get; }

        public Vector2 LocalPosition { get; }

        public Vector2 WorldPosition
        {
            get
            {
                if (Host)
                    return (Vector2) Host.transform.position + LocalPosition;
                else return LocalPosition;
            }
        }

        public IEnumerable<IPathEdge> Edges => edges;
        private List<NestPathEdge> edges = new List<NestPathEdge>();


        /// <summary>
        /// 接続時に使用する識別用のID。
        /// </summary>
        public string Name { get; }

        public bool IsExposed { get; }

        public NestPathNode(NestElement host, Vector2 localPosition, string name = "")
        {
            Host = host;
            LocalPosition = localPosition;
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
                IsExposed = true;
            }
        }

        /// <summary>
        /// あるNestPathNodeとElementを越えた接続が可能であるか調べる。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool IsConnectable(NestPathNode other);

        public void RegisterEdge(NestPathEdge edge)
        {
            if (edge.A != this && edge.B != this)
                throw new ArgumentException("指定したNestPathEdgeはこのNestPathNodeに接続していません");
            if (edges.Contains(edge)) throw new InvalidEnumArgumentException("指定したNestPathEdgeはすでに登録されています。");
            edges.Add(edge);
        }

        public void UnregisterEdge(NestPathEdge edge)
        {
            if (!edges.Contains(edge)) return;
            edges.Remove(edge);
        }
    }
}