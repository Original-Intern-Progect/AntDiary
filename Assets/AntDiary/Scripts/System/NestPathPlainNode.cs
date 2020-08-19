using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// とくに機能を持たないプレーンなNestPathNode。
    /// どのNestPathNodeとも(相手のIsConnectableが許せば)接続可能。
    /// </summary>
    public class NestPathPlainNode : NestPathNode
    {
        public NestPathPlainNode(NestElement host, Vector2 localPosition, string name = "") : base(host, localPosition, name)
        {
        }

        public override bool IsConnectable(NestPathNode other)
        {
            return true;
        }
    }
}