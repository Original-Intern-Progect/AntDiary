using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    [MessagePackObject()]
    public class NestStructureData
    {
        /// <summary>
        /// 道、部屋などの巣の建築要素を保存します。
        /// </summary>
        [Key(0)] public List<NestElementData> NestElements { get; set; } = new List<NestElementData>();
        
        /// <summary>
        /// 道、部屋どうしの接続を保存します。
        /// </summary>
        [Key(1)] public List<NestPathElementEdgeData> ElementEdges { get; set; } = new List<NestPathElementEdgeData>();
        
    }
}