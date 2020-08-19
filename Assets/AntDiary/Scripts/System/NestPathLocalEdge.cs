using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    public class NestPathLocalEdge : NestPathEdge
    {
        public override NestPathNode A { get; }
        public override NestPathNode B { get; }
        
        public NestPathLocalEdge(NestPathNode a, NestPathNode b)
        {
            if(a.Host != b.Host) throw new ArgumentException("異なるNestElementに含まれるNestPathNodeどうしを接続することはできません。");
            A = a;
            B = b;
            
            RegisterToNode();
        }
    }
}