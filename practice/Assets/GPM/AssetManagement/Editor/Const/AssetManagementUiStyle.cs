using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.Const
{
    internal static class UiStyle
    {
        public static readonly GUIStyle GroupBox;
        public static readonly GUIStyle HideBox;
        public static readonly GUIStyle ListBox;
        public static readonly GUIStyle CellBox;
        public static readonly GUIStyle CopyrightBox;
        public static readonly GUIStyle Toolbar;

        public static readonly GUIStyle TitleLabel;
        public static readonly GUIStyle ListLabel;
        
        public static readonly GUIStyle CategoryLabel;
        public static readonly GUIStyle CountLabel;
        public static readonly GUIStyle DuplicateCellLabel;
        public static readonly GUIStyle CopyrightLabel;
        public static readonly GUIStyle VersionLabel;

        public static readonly GUIStyle ToolbarPopup;

        public static readonly GUIStyle ExpandButton;
        public static readonly GUIStyle ToolbarButton;

        public static readonly GUIStyle ListNormalButton;
        public static readonly GUIStyle ListSelectedButton;

        public static readonly GUIStyle TextField;
        public static readonly GUIStyle ToolbarTextField;

        public static readonly GUIStyle Popup;

        public static readonly GUIStyle SubWindow;

        static UiStyle()
        {
            #region Box

            GroupBox = new GUIStyle(GUI.skin.box)
            {
                margin = new RectOffset(6, 6, 6, 6),
            };

            HideBox = new GUIStyle(GUI.skin.box)
            {
                margin = new RectOffset(12, 6, 6, 6),
                normal = { background = null }
            };

            ListBox = new GUIStyle()
            {
                margin = new RectOffset(6, 6, 4, 4),
                padding = new RectOffset(1, 1, 0, 1),
            };

            CellBox = new GUIStyle(GUI.skin.box)
            {
                margin = new RectOffset(4, 2, 4, 2),
                normal = { background = null }
            };

            CopyrightBox = new GUIStyle(GUI.skin.FindStyle("ProgressBarBack"))
            {
                alignment = TextAnchor.LowerCenter,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                stretchHeight = true,
                stretchWidth = true
            };

            Toolbar = new GUIStyle(EditorStyles.toolbar)
            {
                fixedHeight = 20,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
            };

            #endregion

            #region Label

            TitleLabel = new GUIStyle(GUI.skin.label)
            {
                //fontStyle = FontStyle.Bold,
                fontSize = 13,
                margin = new RectOffset(6, 6, 6, 4),
            };

            ListLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                fixedWidth = 178,
                fixedHeight = 20,
                stretchWidth = false
            };

            CategoryLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11
            };

            CountLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 11
            };

            DuplicateCellLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                //fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            CopyrightLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10,
            };

            VersionLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 10,
                normal = { textColor = new Color(100f / 255f, 100f / 255f, 100f / 255f) }
            };

            #endregion

            ToolbarPopup = EditorStyles.toolbarPopup;

            #region Button

            ExpandButton = new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(4, 4, 4, 4),
                padding = new RectOffset(8, 8, 4, 4),
                fixedHeight = 19
            };

            ToolbarButton = new GUIStyle(EditorStyles.toolbarButton)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(8, 8, 4, 4),
                fontSize = 11,
                fixedHeight = 20
            };

            var ListButton = new Texture2D(1, 1);
            ListButton.SetPixel(0, 0, new Color(0.31f, 0.5f, 0.972f, 0.5f));
            ListButton.hideFlags = HideFlags.HideAndDontSave;
            ListButton.name = "OverlayTexture";
            ListButton.Apply();

            ListNormalButton = new GUIStyle(GUI.skin.box)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                fixedHeight = 0f,
                normal = { background = null }
            };

            ListSelectedButton = new GUIStyle(GUI.skin.box)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                fixedHeight = 0f,
                normal = { background = ListButton }
            };

            #endregion

            #region TextField

            TextField = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(4, 4, 5, 4),
                fontSize = 12,
            };

            ToolbarTextField = new GUIStyle(EditorStyles.toolbarTextField)
            {
            };
            
            #endregion

            #region Popup

            Popup = new GUIStyle(EditorStyles.popup)
            {
                margin = new RectOffset(4, 4, 4, 4),
                fixedHeight = 19
            };

            #endregion

            #region Window

            SubWindow = new GUIStyle(GUI.skin.window)
            {
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(12, 12, 12, 12),
                padding = new RectOffset(0, 0, 0, 0)
            };

            #endregion
        }
    }
}