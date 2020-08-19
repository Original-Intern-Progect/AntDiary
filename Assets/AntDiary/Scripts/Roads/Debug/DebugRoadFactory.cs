using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AntDiary
{
    public class DebugRoadFactory : NestElementFactory<DebugRoadData>
    {
        [SerializeField] private GameObject debugRoadPrefab;

        public override NestElement InstantiateNestElement(DebugRoadData elementData)
        {
            var go = Instantiate(debugRoadPrefab, Vector2.Lerp(elementData.From, elementData.To, 0.5f),
                Quaternion.identity);
            var ne = go.GetComponent<DebugRoad>();
            ne.Initialize(elementData);
            return ne;
        }
    }
}