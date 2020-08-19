using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    public abstract class NestElement : MonoBehaviour
    {
        public abstract NestElementData Data { get; }
        
        /// <summary>
        /// 建築時にほかの建築物との重複の有無を判定するのに使用する形状。
        /// </summary>
        /// <returns></returns>
        public abstract Collider2D GetBlockingShape();
        
        public abstract bool HasPathNode { get; }

        public abstract IEnumerable<NestPathNode> GetNodes();
        public abstract IEnumerable<NestPathLocalEdge> GetLocalEdges();
        
        
    }

    public abstract class NestElement<T> : NestElement where T : NestElementData
    {
        
        /// <summary>
        /// 外部公開用のプロパティ
        /// </summary>
        public override NestElementData Data => SelfData;
        
        /// <summary>
        /// クラス内から参照する用のプロパティ。Dataとインスタンスは同一
        /// </summary>
        protected T SelfData { get; private set; }
        protected bool IsInitialized { get; private set; } = false;
        
        /// <summary>
        /// NestSystemがデータを注入する際に使用します。そのほかでは呼ばないでください。 
        /// </summary>
        /// <param name="antData"></param>
        public void Initialize(T antData)
        {
            if (IsInitialized) return;
            SelfData = antData;
            IsInitialized = true;
            gameObject.layer = LayerMask.NameToLayer("NestElement");
            OnInitialized();
        }

        /// <summary>
        /// 初期化が終了（NestElementDataの注入が完了）したタイミングで呼ばれる。
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

    }
}