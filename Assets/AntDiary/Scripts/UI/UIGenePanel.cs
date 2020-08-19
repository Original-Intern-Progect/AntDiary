using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AntDiary
{
    public class UIGenePanel : MonoBehaviour
    {
        [SerializeField] private UIGeneTreeView treeView = default;
        [SerializeField] private GameObject treeButtonPrefab = default;
        [SerializeField] private RectTransform treeButtonRoot = default;
        
        public GeneTree SelectedGeneTree { get; private set; }
        public IObservable<GeneTree> OnSelectedGeneTreeChanged => onSelectedGeneTreeChanged;
        private Subject<GeneTree> onSelectedGeneTreeChanged = new Subject<GeneTree>();
        
        // Start is called before the first frame update
        void Start()
        {
            foreach (var t in GeneSystem.Instance.Trees)
            {
                var g = Instantiate(treeButtonPrefab, treeButtonRoot);
                g.GetComponent<UIGenePanelTreeSelectButton>().Initialize(this, t);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            //SelectGeneTree(GeneSystem.Instance.Trees.First());
        }

        public void SelectGeneTree(GeneTree tree)
        {
            if (SelectedGeneTree == tree) return;
            SelectedGeneTree = tree;
            treeView.Build(tree);
            onSelectedGeneTreeChanged.OnNext(tree);
        }

        public void ActivateSelectedGene()
        {
            var g = treeView.SelectedGene;
            if (g == null) return;
            GeneSystem.Instance.Activate(SelectedGeneTree, g);
        }
        
    }
}