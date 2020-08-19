using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{

    public abstract class Ant : MonoBehaviour
    {
        public abstract AntData Data { get; }
    }
    /// <summary>
    /// シーン上に配置されるアリにとりつけるComponent
    /// </summary>
    public abstract class Ant<T> : Ant where T : AntData
    {
        /// <summary>
        /// 外部公開用のプロパティ
        /// </summary>
        public override AntData Data => SelfData;
        
        /// <summary>
        /// クラス内から参照する用のプロパティ。Dataとインスタンスは同一
        /// </summary>
        protected T SelfData { get; private set; }
        protected bool IsInitialized { get; private set; } = false;
        
        /// <summary>
        /// NestSystemがデータを注入するのに使用します。そのほかからは呼ばないでください。
        /// </summary>
        /// <param name="antData"></param>
        public void Initialize(T antData)
        {
            if (IsInitialized) return;
            SelfData = antData;
            IsInitialized = true;
            OnInitialized();
        }

        /// <summary>
        /// 初期化が終了（AntDataの注入が完了）したタイミングで呼ばれる。
        /// </summary>
        protected virtual void OnInitialized()
        {
        }
    }
}