using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// NestSystemの保持するデータ。
    /// </summary>
    [MessagePackObject()]
    public class NestData
    {
        [Key(10)]
        public List<AntData> Ants { get; set; } = new List<AntData>();
        [Key(11)]
        public int StoredFood { get; set; }
        [Key(12)]
        public NestStructureData Structure { get; set; } = new NestStructureData();

    }
}