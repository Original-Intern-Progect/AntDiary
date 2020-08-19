using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    [MessagePackObject()]
    public class DebugRoadData : NestElementData
    {
        [Key(10)] public Vector2 From { get; set; }
        [Key(11)] public Vector2 To { get; set; }
    }
}