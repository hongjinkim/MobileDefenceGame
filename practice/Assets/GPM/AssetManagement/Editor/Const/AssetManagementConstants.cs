using UnityEngine;

using Gpm.Common.Multilanguage;

namespace Gpm.AssetManagement.Const
{
    public static class Constants
    {
        public const string SERVICE_NAME = "AssetManagement";

        public const string LANGUAGE_FILE_PATH = "GPM/AssetManagement/XML/Language.xml";

        public const string PATH_ASSET = "Assets/";
        public const string PATH_TRASH = "Trash/";

        public const string PATH_GPM = "GPM/";
        public const string FILENAME_UNUSEDASSET_FILTER = "UnusedAssetFilter.bytes";

        private const string LANGUAGE_CODE_KEY = "Gpm.assetmanagement.language";

        private const string GIT_URL = "https://github.com/nhn/gpm.unity/blob/master/docs/AssetManagement";


        public const string FORMAT_MOVE_ASSET = "movedAssets({0}/{1})";
        public const string FORMAT_IMPORT_ASSET = "importedAssets({0}/{1})";
        public const string FORMAT_DELETED_ASSET = "deletedAssets({0}/{1})";
        public const string FORMAT_MISSING_CHECK = "missingCheck({0}/{1})";
        public const string FORMAT_UNKNOWN_CHECK = "unknownCheck({0}/{1})";
        public const string FORMAT_REFERENCE_CHECK = "referenceCheck({0}/{1})";
        public const string FORMAT_UNUSEDASSET_CHECK = "unusedAssetCheck({0}/{1})";

        public const string FORMAT_CACHE_ASSETDATA = "CacheAssetData({0}/{1})";

        public const string FORMAT_NOT_FOUND_GUID = "Not Found guid - {0}";
        public const string FORMAT_NOT_FOUND_DEPENDENCY = "Not Found AssetMap(Retry call CacheAssetDataAll) - {0}";

        public const string FORMAT_MISSING_DEPENDENCY_MESSAGE = "{0} missing Dependency({1})";
        public const string FORMAT_IS_NOT_FILE = "{0} is not file";

        public const string FORMAT_ITEM_INVALID_NAME = "{0} - NULL";

        public const string ICON_WARN = "console.warnicon.sml";
        public const string ICON_UNITYLOGO = "BuildSettings.Editor.Small";

        public const string GUI_STYLE_HIGHLIGHT = "LightmapEditorSelectedHighlight";
        public const string GUI_TEXTURE_TYPE = "FilterByType";

        public static string DOCUMENT_URL
        {
            get
            {
                string languageCode = GpmMultilanguage.GetSelectLanguage(SERVICE_NAME, false);
                return (languageCode.Equals("ko") == true) ?
                    string.Format("{0}/README.md", GIT_URL) :
                    string.Format("{0}/README.{1}.md", GIT_URL, languageCode);
            }
        }

        public static string LastLanguageName
        {
            get
            {
                return PlayerPrefs.GetString(LANGUAGE_CODE_KEY);
            }
            set
            {
                PlayerPrefs.SetString(LANGUAGE_CODE_KEY, value);
            }
        }

        public const float CHECK_TIME_PROGRESS = 0.25f;
    }
}
