using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// 経路探索システムで使用するノード。
    /// </summary>
    public interface IPathNode
    {
        /// <summary>
        /// ワールド座標。
        /// </summary>
        Vector2 WorldPosition { get; }
        
        /// <summary>
        /// このノードと接続するEdgeのリスト。
        /// </summary>
        IEnumerable<IPathEdge> Edges { get; }
    }
}