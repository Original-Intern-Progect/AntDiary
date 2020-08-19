using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntDiary
{
    [CreateAssetMenu(menuName = "AntDiary/GeneTree", fileName = "GeneTree")]
    public class GeneTree : ScriptableObject
    {
        [SerializeField] private string displayName = default;
        [SerializeField] private List<Gene> genes = new List<Gene>();
        [SerializeField] private List<GeneEdge> edges = new List<GeneEdge>();

        public string DisplayName => displayName;

        public List<Gene> Genes => genes;
        public List<GeneEdge> Edges => edges;

        /// <summary>
        /// Gene間の接続を構成する。
        /// </summary>
        public void ApplyEdges()
        {
            foreach (var edge in edges)
            {
                var parentGene = genes.FirstOrDefault(g => g.Guid == edge.ParentGeneGuid);
                if (parentGene == default) continue;
                var childGene = genes.FirstOrDefault(g => g.Guid == edge.ChildGeneGuid);
                if (childGene == default) continue;

                if (childGene.ParentGene != null)
                {
                    Debug.LogWarning("GeneTreeに親子関係の重複があります。データに誤りが無いか確認してください。");
                    continue;
                }

                parentGene.ChildGenes.Add(childGene);
                childGene.ParentGene = parentGene;
            }
        }
    }

    /// <summary>
    /// 遺伝子一つを示す
    /// </summary>
    [Serializable]
    public class Gene
    {
        /// <summary>
        /// 任意に設定できるID。解放時の動作を定義する際に使用。
        /// </summary>
        [SerializeField] private string id;

        public string Id
        {
            get => id;
            set => id = value;
        }

        [SerializeField] private string displayName = "遺伝子";

        public string DisplayName
        {
            get => displayName;
            set => displayName = value;
        }

        [SerializeField] private string description;

        public string Description
        {
            get => description;
            set => description = value;
        }

        /// <summary>
        /// 自動的に付与される遺伝子固有のGUID。識別に使用。
        /// </summary>
        [SerializeField] private string guid;

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        //エディタ用

        [SerializeField] private Rect position;

        public Rect Position
        {
            get => position;
            set => position = value;
        }


        public Gene()
        {
            guid = System.Guid.NewGuid().ToString();
        }

        //Runtime Only
        public bool IsRootGene => ParentGene == null;
        public Gene ParentGene { get; set; }
        [field:NonSerialized] public List<Gene> ChildGenes { get; set; } = new List<Gene>();

        public bool IsActivated => GameContext.Current.s_GeneData.ActivatedGenes.Contains(guid);
    }

    /// <summary>
    /// Gene間の接続を示す
    /// </summary>
    [Serializable]
    public class GeneEdge
    {
        [SerializeField] private string parentGeneGuid;

        public string ParentGeneGuid
        {
            get => parentGeneGuid;
            set => parentGeneGuid = value;
        }

        [SerializeField] private string childGeneGuid;

        public string ChildGeneGuid
        {
            get => childGeneGuid;
            set => childGeneGuid = value;
        }
    }
}