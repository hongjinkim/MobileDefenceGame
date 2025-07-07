namespace Gpm.LogViewer.Internal
{
    using UnityEngine;
    
    public class Opener : MonoBehaviour
    {
        public Viewer viewer;
        
        public Vector2 showPostion =  new Vector2(30, -30);
        
        private RectTransform openerTransform;
        private bool isDragging = false;
        
        private static Opener   instance    = null;
        
        public static Opener Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Opener>();
                }

                return instance;
            }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            openerTransform = GetComponent<RectTransform>();
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);

            if (show == true)
            {
                openerTransform.anchoredPosition = showPostion;
            }
        }

        public void Open()
        {
            if (isDragging == false)
            {
                viewer.Show();
            }
        }
        
        public void OnBeginDrag()
        {
            isDragging = true;
        }
        
        public void OnEngDrag()
        {
            isDragging = false;
        }
    }
}