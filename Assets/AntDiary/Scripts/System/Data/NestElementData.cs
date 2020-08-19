using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    [Union(0, typeof(DebugRoomData))]
    [Union(1, typeof(DebugRoadData))]
    public abstract class NestElementData
    {
        [Key(0)] public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        [Key(1)] public Vector2 Position { get; set; }
    }
}