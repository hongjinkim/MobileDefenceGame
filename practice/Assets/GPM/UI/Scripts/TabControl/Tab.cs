namespace Gpm.Ui
{
    using DG.Tweening;
    using Internal;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public interface ITabData
    {
    }

    public class Tab : TabLinkObject
    {
        [Header("State", order = 0)]
        [SerializeField]
        private bool blockTab = false;

        [SerializeField]
        private bool selected = false;

        [Header("Event", order = 2)]
        public UpdateDataEvent onUpdateData = new UpdateDataEvent();
        public ChangeValueEvent onChangeValue = new ChangeValueEvent();
        public ChangeValueEvent onChangeBlock = new ChangeValueEvent();

        public TabEvent onSelected = new TabEvent();
        public TabEvent onBlocked = new TabEvent();

        private ITabData data = null;

        [Header("Layout")]
        private Vector3 defaultScale = Vector3.one;
        private LayoutElement le;
        private RectTransform rt;

        private void Awake()
        {
            le = GetComponent<LayoutElement>();
            rt = GetComponent<RectTransform>();
        }


        public ITabData GetData()
        {
            return data;
        }

        public void SetData(ITabData data, bool notify = true)
        {
            this.data = data;

            OnUpdateData(data);

            if (notify == true &&
                IsSelected() == true)
            {
                NotifyPage();
            }
        }

        public TabPage GetLinkedPage()
        {
            TabGroup group = GetGroup();
            if (group != null)
            {
                return group.Get(this).GetPage();
            }

            return null;
        }

        public void SetLinkPage(TabPage page)
        {
            TabGroup group = GetGroup();
            if (group != null)
            {
                group.Set(this, page);
            }
        }

        public virtual void OnClick()
        {
            Select();
        }

        public bool IsBlock()
        {
            return blockTab;
        }

        public void SetBlockTab(bool value)
        {
            blockTab = value;

            if (onChangeBlock != null)
            {
                onChangeBlock.Invoke(blockTab);
            }
        }

        public bool IsSelected()
        {
            return selected == true && GetActive() == true;
        }
        public void Select()
        {
            TabGroup group = GetGroup();
            if (group != null)
            {
                group.Select(this);
            }
            else
            {
                if (blockTab == true)
                {
                    OnBlocked();
                    return;
                }

                SetValue(true);
            }
        }

        public void NotifyPage()
        {
            ITabPage tabPage = GetLinkedPage();
            if (tabPage != null)
            {
                tabPage.Notify(this);
            }
        }

        internal void SetValue(bool selected)
        {
            this.selected = selected;

            if (selected == true)
            {
                OnSelected();
            }

            OnChangeValue(selected);
        }

        private void UpdateState()
        {
            SetValue(selected);
            SetBlockTab(blockTab);
        }

        public virtual void OnUpdateData(ITabData data)
        {
            if (onUpdateData != null)
            {
                onUpdateData.Invoke(data);
            }
        }

        public virtual void OnChangeValue(bool selected)
        {
            if (onChangeValue != null)
            {
                onChangeValue.Invoke(selected);
            }
        }

        public virtual void OnSelected()
        {
            if (onSelected != null)
            {
                onSelected.Invoke(this);
            }
        }

        public virtual void OnBlocked()
        {
            if (onBlocked != null)
            {
                onBlocked.Invoke(this);
            }
        }

        public void HighlightSelectedTab()
        {
            if (le == null)
            {
                le = GetComponent<LayoutElement>();
            }

            
            float targetFlexibleWidth = selected ? 1.4f : 1f;
            DOTween.To(() => le.flexibleWidth,
                       v => le.flexibleWidth = v,
                       targetFlexibleWidth,
                       0.2f)
                   .SetEase(Ease.OutCubic);

            float targetHeight = selected ? 190f : 170f; // 원하는 높이 값으로 변경
            DOTween.To(() => rt.sizeDelta.y,
                       v => rt.sizeDelta = new Vector2(rt.sizeDelta.x, v),
                       targetHeight,
                       0.2f)
                   .SetEase(Ease.OutCubic);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            SetValue(selected);
            SetBlockTab(blockTab);
        }
#endif

        [System.Serializable]
        public class UpdateDataEvent : UnityEvent<ITabData>
        {
            public UpdateDataEvent()
            {

            }
        }

        [System.Serializable]
        public class ChangeValueEvent : UnityEvent<bool>
        {
            public ChangeValueEvent()
            {

            }
        }

        [System.Serializable]
        public class TabEvent : UnityEvent<Tab>
        {
            public TabEvent()
            {

            }
        }
    }
}
