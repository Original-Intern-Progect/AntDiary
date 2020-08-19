using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntDiary
{
    public static class PathNodeExtensions
    {
        public static IEnumerable<IPathNode> GetConnectedNodes(this IPathNode target)
        {
            return target.Edges.Select(e => e.GetOtherNode(target));
        }

        public static IPathNode GetOtherNode(this IPathEdge target, IPathNode one)
        {
            if (target.A == one) return target.B;
            if (target.B == one) return target.A;
            throw new ArgumentException("IPathEdgeは指定されたIPathNodeを含んでいません。");
        }
    }
}