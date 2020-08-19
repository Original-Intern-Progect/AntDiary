using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// 経路探索システムで使用する接続。
    /// </summary>
    public interface IPathEdge
    {
        IPathNode A { get; }
        IPathNode B { get; }
        float Cost { get; }
    }
}