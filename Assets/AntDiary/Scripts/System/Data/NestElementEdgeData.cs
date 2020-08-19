using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    [MessagePackObject()]
    public class NestPathElementEdgeData
    {
        [Key(0)] public string ElementGuidA { get; set; }
        [Key(1)] public string NodeNameA { get; set; }
        [Key(2)] public string ElementGuidB { get; set; }
        [Key(3)] public string NodeNameB { get; set; }
        
        
    }
}
