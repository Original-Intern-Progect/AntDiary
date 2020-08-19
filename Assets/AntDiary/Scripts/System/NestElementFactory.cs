using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// NestElementDataから実際のGameObjectを作成するクラス。
    /// </summary>
    public abstract class NestElementFactory : MonoBehaviour
    {
        public abstract Type DataType { get; }
        
        /// <summary>
        /// AntDataをもとに、シーン上にAntのGameObjectを生成します。
        /// </summary>
        /// <param name="elementData"></param>
        /// <returns></returns>
        public abstract NestElement InstantiateNestElement(NestElementData elementData);
    }
    
    public abstract class NestElementFactory<T> : NestElementFactory where T : NestElementData
    {
        public sealed override Type DataType => typeof(T);

        public sealed override NestElement InstantiateNestElement(NestElementData elementData)
        {
            if(elementData is T elementDataCast)
            {
                return InstantiateNestElement(elementDataCast);
            }
            else
            {
                throw new ArgumentException($"Type mismatch. Expected type is {typeof(T).Name} but {elementData.GetType().Name} is specified.");
            }
        }

        /// <summary>
        /// NestElementDataをもとに、シーン上にNestElementのGameObjectを生成します。
        /// </summary>
        /// <param name="elementData"></param>
        /// <returns>生成されたGameObjectのもつNestElementコンポーネント。</returns>
        public abstract NestElement InstantiateNestElement(T elementData);
    }
}