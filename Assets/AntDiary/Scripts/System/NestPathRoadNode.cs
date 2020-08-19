using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// 道用のNestPathNode
    /// </summary>
    public class NestPathRoadNode : NestPathNode
    {
        public NestPathRoadNode(NestElement host, Vector2 localPosition, string name = "") : base(host, localPosition, name)
        {
        }
        
        public override bool IsConnectable(NestPathNode other)
        {
            return true;
        }
    }
}