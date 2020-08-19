using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    public class DebugRoomFactory : NestElementFactory<DebugRoomData>
    {
        [SerializeField] private GameObject debugRoomPrefab = default;

        public override NestElement InstantiateNestElement(DebugRoomData elementData)
        {
            var r = Instantiate(debugRoomPrefab, elementData.Position, Quaternion.identity)
                .GetComponent<DebugRoom>();
            r.Initialize(elementData);
            return r;
        }
    }
}