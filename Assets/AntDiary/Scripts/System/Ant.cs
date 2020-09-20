using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AntDiary
{
    public abstract class Ant : MonoBehaviour
    {
        public abstract AntData Data { get; }
        
        public abstract AntCommonDataBase CommonData { get; }

        protected abstract float MovementSpeed { get; }

        protected virtual void Update()
        {
            UpdateMovementOnPath();
        }

        private void UpdateMovementOnPath()
        {
            if (NextNode != null)
            {
                var d = NextNode.WorldPosition - (Vector2) transform.position;
                float step = MovementSpeed * Time.deltaTime;
                if (d.sqrMagnitude <= step * step)
                {
                    transform.position = NextNode.WorldPosition;
                    CurrentNode = NextNode;
                    NextNode = null;
                    if (CurrentNode == CurrentTargetNode)
                    {
                        //到着
                        var a = OnArrived;
                        OnArrived = null;
                        OnAborted = null;
                        a?.Invoke();
                    }
                    else
                    {
                        UpdatePath();
                    }
                }
                else
                {
                    transform.position = (Vector2) transform.position + d.normalized * step;
                }
            }
        }

        /// <summary>
        /// 現在の目的地。
        /// </summary>
        protected NestPathNode CurrentTargetNode { get; private set; }

        /// <summary>
        /// 現在の経路で、次に通過する予定のノード。
        /// </summary>
        protected NestPathNode NextNode { get; private set; }

        /// <summary>
        /// 現在位置している、または最後に位置したノード。
        /// </summary>
        protected NestPathNode CurrentNode { get; private set; }

        private Action OnArrived { get; set; }
        private Action OnAborted { get; set; }

        /// <summary>
        /// 目的のNodeへの自動移動を開始する。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onArrived">ノードへ到着した際のコールバック。</param>
        /// <param name="onAborted">何らかの理由で移動が中止された際のコールバック。</param>
        public void StartForPathNode(NestPathNode node, Action onArrived = null, Action onAborted = null)
        {
            CancelMovement();
            
            OnAborted = onAborted;
            OnArrived = onArrived;

            CurrentTargetNode = node;
            UpdatePath();
        }

        private void UpdatePath()
        {
            //現在位置ノードが不明な場合や、既知のCurrentNodeが実際の現在位置から離れすぎている場合は、現在位置ノードを再計算
            if (CurrentNode == null || (CurrentNode.WorldPosition - (Vector2) transform.position).sqrMagnitude > 1f){
                //現在位置が不明なので、検索
                var argMinSqrDistance = SupposeCurrentPosNode();

                //とりあえず一番近いノードに移動
                NextNode = argMinSqrDistance ?? throw new NullReferenceException("巣にIPathNodeが見つかりませんでした。");
            }
            else
            {
                if (CurrentTargetNode == null) throw new NullReferenceException("ターゲットのノードを指定してください。");
                var route = NestSystem.Instance.FindRoute(CurrentNode, CurrentTargetNode);
                if (route.Count() < 2)
                {
                    //ルート計算に失敗。接続されていない？
                    CurrentTargetNode = null;
                    var a = OnAborted;
                    OnArrived = null;
                    OnAborted = null;
                    a?.Invoke();
                    return;
                }

                if (route.ElementAt(1) is NestPathNode n)
                {
                    NextNode = n;
                }
                else throw new InvalidOperationException("ノードはNestPathNodeではない。");
            }
        }

        /// <summary>
        /// 現在の座標から今いると思われるNodeを推定する
        /// </summary>
        /// <returns>最も近いNode</returns>
        public NestPathNode SupposeCurrentPosNode(){
            float Distance(IPathNode node) => Vector2.Distance(node.WorldPosition, transform.position);

            if (!NestSystem.Instance.NestPathNodes.Any()) return null;
            
            return NestSystem.Instance.NestPathNodes
                .Aggregate((result, next) => Distance(next) < Distance(result) ? next : result);
        }

        public void CancelMovement()
        {
            CurrentTargetNode = null;
            NextNode = null;
            CurrentNode = null;
            
            var a = OnAborted;
            OnArrived = null;
            OnAborted = null;
            a?.Invoke();
        }
    }

    /// <summary>
    /// シーン上に配置されるアリにとりつけるComponent
    /// </summary>
    public abstract class Ant<T, TCommonData> : Ant where T : AntData where TCommonData : AntCommonData<T>
    {
        /// <summary>
        /// 外部公開用のプロパティ
        /// </summary>
        public override AntData Data => SelfData;

        /// <summary>
        /// クラス内から参照する用のプロパティ。Dataとインスタンスは同一
        /// </summary>
        protected T SelfData { get; private set; }

        public override AntCommonDataBase CommonData => SelfCommonData;

        protected TCommonData SelfCommonData { get; private set; }

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
            
            //位置データをセーブデータと同期
            SaveUnit.Current.OnBeforeSave.Subscribe(_ => Data.Position = transform.position);
            SaveUnit.OnCurrentSaveUnitChanged.Subscribe(_ =>
            {
                SaveUnit.Current.OnBeforeSave.Subscribe(__ => Data.Position = transform.position);
            });

            SelfCommonData = GetCommonData();

            //体力を最大に設定
            Data.Health = CommonData.MaxHealth;
            
            OnInitialized();
        }

        protected virtual TCommonData GetCommonData()
        {
            if (!NestSystem.Instance.Data.CommonDataRegistry.TryGetCommonData(out TCommonData selfCommonData))
            {
                Debug.LogWarning($"セーブデータに{typeof(T).Name}用のAntCommonDataが見つかりませんでした。");
                throw new NullReferenceException();
            }

            return selfCommonData;
        }
        

        /// <summary>
        /// 初期化が終了（AntDataの注入が完了）したタイミングで呼ばれる。
        /// </summary>
        protected virtual void OnInitialized()
        {
        }
    }

    public abstract class Ant<T> : Ant<T, AntCommonData<T>> where T : AntData
    {
        protected override AntCommonData<T> GetCommonData()
        {
            if (!NestSystem.Instance.Data.CommonDataRegistry.TryGetCommonData(out AntCommonData<T> selfCommonData))
            {
                Debug.LogWarning($"セーブデータに{typeof(T).Name}用のAntCommonDataが見つかりませんでした。代わりに仮のデータを使用します。");
                selfCommonData = new AntCommonData<T>();
            }

            return selfCommonData;
        }
    }
}