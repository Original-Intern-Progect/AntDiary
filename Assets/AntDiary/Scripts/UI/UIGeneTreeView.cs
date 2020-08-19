using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AntDiary
{
    /// <summary>
    /// 遺伝子ツリーの表示を行います。
    /// 遺伝子ツリーの選択とかステータスとかはUIGenePanelが包括して行います
    /// </summary>
    public class UIGeneTreeView : MonoBehaviour
    {
        [SerializeField] private Transform elementsRoot = default;
        [SerializeField] private GameObject geneTreeNodePrefab = default;
        [SerializeField] private GameObject geneTreeEdgePrefab = default;
        [SerializeField] private RectTransform maskContainer = default;
        [SerializeField] private RectTransform contentContainer = default;
        [SerializeField] private float padding = 200f;
        [SerializeField] private float horizontalSpacing = 200f;
        [SerializeField] private float verticalSpacing = 100f;

        private List<UIGeneTreeNode> treeNodes = new List<UIGeneTreeNode>();
        private List<UIGeneTreeEdge> treeEdges = new List<UIGeneTreeEdge>();
        
        public GeneTree CurrentGeneTree { get; private set; }
        public Gene SelectedGene { get; private set; }

        public IObservable<Gene> OnSelectedGeneChanged => onSelectedGeneChanged;
        private Subject<Gene> onSelectedGeneChanged = new Subject<Gene>();
        
        
        /// <summary>
        /// 指定したGeneを選択状態にする。
        /// </summary>
        /// <param name="gene"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SelectGene(Gene gene)
        {
            if (!CurrentGeneTree) return;
            if(!CurrentGeneTree.Genes.Contains(gene)) throw new ArgumentException("指定したGeneは現在のGeneTreeに含まれていません。");
            if (SelectedGene == gene) return;
            SelectedGene = gene;
            onSelectedGeneChanged.OnNext(gene);
        }


        /// <summary>
        /// 指定したGeneTreeをもとにツリーのUIを構築します。
        /// </summary>
        /// <param name="geneTree"></param>
        public void Build(GeneTree geneTree)
        {
            foreach (var node in treeNodes)
            {
                Destroy(node.gameObject);
            }

            treeNodes.Clear();

            foreach (var edge in treeEdges)
            {
                Destroy(edge.gameObject);
            }

            treeEdges.Clear();

            CurrentGeneTree = geneTree;
            SelectedGene = null;

            var rootNodes = geneTree.Genes.Where(g => g.IsRootGene);
            //Depth, Orderを計算
            List<int> orders = new List<int>();
            List<UIGeneTreeNode> ends = new List<UIGeneTreeNode>();

            foreach (var gene in rootNodes)
            {
                var n = SetupGeneNodeKernel(orders, ends, 0, gene);
                PlaceGeneNodeKernel(ends, n, 0);
            }

            float boundWidth = (orders.Count - 1) * horizontalSpacing + padding * 2f;
            float boundHeight = (ends.Count - 1) * verticalSpacing + padding * 2f;
            
            
            contentContainer.sizeDelta = new Vector2(boundWidth, boundHeight);

        }

        private UIGeneTreeNode SetupGeneNodeKernel(List<int> orders, List<UIGeneTreeNode> ends, int depth, Gene gene)
        {
            if (orders.Count <= depth) orders.Add(0);
            
            UIGeneTreeNode n = Instantiate(geneTreeNodePrefab, elementsRoot).GetComponent<UIGeneTreeNode>();
            n.Initialize(this, gene, depth, orders[depth]);
            treeNodes.Add(n);
            
            orders[depth]++;
            if (gene.ChildGenes.Count == 0)
            {
                ends.Add(n);
            }
            else
            {
                foreach (var child in gene.ChildGenes)
                {
                    var childNode = SetupGeneNodeKernel(orders, ends, depth + 1, child);
                    n.ChildGeneNodes.Add(childNode);
                }
            }

            return n;
        }

        private Vector2 PlaceGeneNodeKernel(List<UIGeneTreeNode> ends, UIGeneTreeNode node, int depth)
        {
            if (node.ChildGeneNodes.Count == 0)
            {
                //End
                Vector2 p = new Vector2( padding + depth * horizontalSpacing, padding + ends.IndexOf(node) * verticalSpacing);
                node.Position = p;
                return p;
            }
            else
            {
                Vector2 sum = Vector2.zero;
                int i = 0;
                foreach (var child in node.ChildGeneNodes)
                {
                    sum += PlaceGeneNodeKernel(ends, child, depth + 1);
                    i++;
                }

                sum /= i;
                Vector2 p = new Vector2( padding + depth * horizontalSpacing, sum.y);
                node.Position = p;
                
                foreach(var child in node.ChildGeneNodes)
                {
                    
                    UIGeneTreeEdge e = Instantiate(geneTreeEdgePrefab, elementsRoot).GetComponent<UIGeneTreeEdge>();
                    e.RectTransform.SetAsFirstSibling();
                    e.SetTarget(node, child);
                    treeEdges.Add(e);
                    
                }
                return p;
            }
        }

        private Vector2 dragStartMousePosition;
        private Vector2 dragStartContainerPosition;
        public void BeginDragContentContainer(BaseEventData evt)
        {
            dragStartMousePosition = evt.currentInputModule.input.mousePosition;
            dragStartContainerPosition = contentContainer.anchoredPosition;
            evt.Use();
        }
        
        public void DragContentContainer(BaseEventData evt)
        {
            Vector2 p = evt.currentInputModule.input.mousePosition - dragStartMousePosition +
                        dragStartContainerPosition;
            p.x = Mathf.Clamp(p.x,  -contentContainer.sizeDelta.x + maskContainer.rect.width, 0);
            p.y = Mathf.Clamp(p.y,  -contentContainer.sizeDelta.y + maskContainer.rect.height, 0);
            contentContainer.anchoredPosition = p;
            
            evt.Use();
        }
        
        public void EndDragContentContainer(BaseEventData evt)
        {
            
        }
        

        public void SelectGene()
        {
        }
    }
}