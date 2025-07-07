using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.Const
{
    internal static class UiContent
    {
        public static readonly GUIContent TreeTabNameContents;
        public static readonly GUIContent TreeTabTypeContents;
        public static readonly GUIContent TreeTabFunctionContents;

        public static readonly GUIContent WarnIconContents;

        static UiContent()
        {
            #region Tree
            TreeTabNameContents = new GUIContent(Strings.TEXT_TREE_TAB_NAME, Strings.TEXT_TREE_TAB_NAME_TOOLTIP);
            TreeTabTypeContents = new GUIContent(EditorGUIUtility.FindTexture(Constants.GUI_TEXTURE_TYPE), Strings.TEXT_TREE_TAB_TYPE_TOOLTIP);
            TreeTabFunctionContents = new GUIContent(Strings.TEXT_TREE_TAB_FUNCTION, Strings.TEXT_TREE_TAB_FUNCTION_TOOLTIP);

            WarnIconContents = EditorGUIUtility.IconContent(Constants.ICON_WARN);
            #endregion
        }
    }
}