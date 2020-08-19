using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AntDiary
{
    /// <summary>
    /// UIGeneTreeViewに表示される遺伝子ノードを示します。
    /// </summary>
    public class UIGeneTreeNode : MonoBehaviour
    {
        public void Initialize(UIGeneTreeView view, Gene targetGene, int depth, int order)
        {
            TargetGene = targetGene;
            ParentView = view;
            Depth = depth;
            Order = order;

            ParentView.OnSelectedGeneChanged.Subscribe(gene =>
            {
                bool isSelected = gene == TargetGene;
                if (IsSelected == isSelected) return;
                IsSelected = isSelected;
                if (isSelected) OnSelected();
                else OnUnselected();
            }).AddTo(this);

            if (TargetGene.IsActivated)
            {
                Image.sprite = selectedSprite;
            }
            else
            {
                GeneSystem.Instance.OnGeneActivated.Subscribe(gene =>
                {
                    if (gene == TargetGene)
                    {
                        Image.sprite = selectedSprite;
                    }
                });
            }
        }

        public Gene TargetGene { get; private set; }

        public UIGeneTreeView ParentView { get; private set; }
        public int Depth { get; set; }
        public int Order { get; set; }

        private RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (!rectTransform) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }


        private Image image;

        public Image Image
        {
            get
            {
                if (!image) image = GetComponent<Image>();
                return image;
            }
        }
        
        private Outline outline;

        public Outline Outline
        {
            get
            {
                if (!outline) outline = GetComponent<Outline>();
                return outline;
            }
        }

        public Vector2 Position
        {
            get => RectTransform.anchoredPosition;
            set => RectTransform.anchoredPosition = value;
        }


        public List<UIGeneTreeNode> ChildGeneNodes { get; } = new List<UIGeneTreeNode>();

        public bool IsSelected { get; private set; } = false;

        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite selectedSprite = default;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            ParentView.SelectGene(TargetGene);
        }

        private void OnSelected()
        {
            Outline.enabled = true;
        }

        private void OnUnselected()
        {
            Outline.enabled = false;
        }
    }
}