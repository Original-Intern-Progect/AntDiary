using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// 部屋用のNestPathNode
    /// </summary>
    public class NestPathRoomNode : NestPathNode
    {
        public NestPathRoomNode(NestElement host, Vector2 localPosition, string name = "") : base(host, localPosition, name)
        {
        }

        public override bool IsConnectable(NestPathNode other)
        {
            //道のノードとだけ接続可能
            if (other is NestPathRoadNode)
            {
                switch (other.Name)
                {
                    case "right":
                        return this.Name == "left";
                    case "top":
                        return this.Name == "bottom";
                    case "left":
                        return this.Name == "right";
                    case "bottom":
                        return this.Name == "top";
                    default:
                        return true;
                    
                }
            }

            return false;
        }
        
    }
}