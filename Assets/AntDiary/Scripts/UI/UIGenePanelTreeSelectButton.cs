using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AntDiary
{
    public class UIGenePanelTreeSelectButton : MonoBehaviour
    {
        private UIGenePanel panel;
        private GeneTree tree;
        private Image image;

        public Image Image
        {
            get
            {
                if (!image) image = GetComponent<Image>();
                return image;
            }
        }
        
        [SerializeField] private Sprite selectedSprite = default;
        [SerializeField] private Sprite defaultSprite = default;
        
        public void Initialize(UIGenePanel panel, GeneTree tree)
        {
            this.panel = panel;
            this.tree = tree;
            
            GetComponentInChildren<Text>().text = tree.DisplayName;
            GetComponent<Button>().onClick.AddListener(OnClick);

            panel.OnSelectedGeneTreeChanged.Subscribe(t =>
            {
                if (t == tree)
                {
                    Image.sprite = selectedSprite;
                }
                else
                {
                    Image.sprite = defaultSprite;
                }
            });
        }

        private void OnClick()
        {
            panel.SelectGeneTree(tree);
        }
    }
}
