using UnityEngine;
using UnityEditor;

using Gpm.Common.Multilanguage;
using Gpm.Common.Log;

namespace Gpm.AssetManagement.Const
{
    static public class AssetManagementLanguage
    {
        private const string EMPTY_LANGUAGES_VALUE = "-";
        private const int LANGUAGE_NOT_FOUND = -1;

        private static string[] languages;
        private static int selectedLanguageIndex;

        public static void Load(System.Action callback)
        {
            LanguageLoad( () =>
            {
                InitializeLanguage(callback, true);
            });
        }

        public static void LanguageLoad(System.Action callback)
        {
            if (GpmMultilanguage.IsLoadService(Constants.SERVICE_NAME) == false)
            {
                GpmMultilanguage.Load(
                Constants.SERVICE_NAME,
                Constants.LANGUAGE_FILE_PATH,
                (result, resultMsg) =>
                {
                    if (result != MultilanguageResultCode.SUCCESS && result != MultilanguageResultCode.ALREADY_LOADED)
                    {
                        GpmLogger.Error(string.Format("Language load failed. (type= {0})", result), Constants.SERVICE_NAME, typeof(AssetManagementLanguage));
                        return;
                    }

                    callback();
                });
            }
            else
            {
                callback();
            }
        }

        public static void InitializeLanguage(System.Action callback, bool load = false)
        {
            if (languages != null)
            {
                callback();
                return;
            }

            if (GpmMultilanguage.IsLoadService(Constants.SERVICE_NAME) == true)
            {
                languages = GpmMultilanguage.GetSupportLanguages(Constants.SERVICE_NAME, true);
                if (languages != null)
                {
                    string lastLanguageName = Constants.LastLanguageName;
                    if (string.IsNullOrEmpty(lastLanguageName) == false)
                    {
                        GpmMultilanguage.SelectLanguageByNativeName(
                            Constants.SERVICE_NAME,
                            lastLanguageName,
                            (result, resultMessage) =>
                            {
                                if (result != MultilanguageResultCode.SUCCESS)
                                {
                                    GpmLogger.Warn(
                                        string.Format("{0} (Code= {1})", Ui.GetString(Strings.KEY_CHANGE_LANGUAGE_ERROR_MESSAGE), result),
                                        Constants.SERVICE_NAME,
                                        typeof(AssetManagementLanguage));
                                }
                            });
                    }

                    selectedLanguageIndex = GetSelectLanguageIndex(GpmMultilanguage.GetSelectLanguage(Constants.SERVICE_NAME, true));
                }
                else
                {
                    languages = new[] { EMPTY_LANGUAGES_VALUE };
                    selectedLanguageIndex = 0;
                }

                callback();
            }
            else
            {
                languages = new[] { EMPTY_LANGUAGES_VALUE };
                selectedLanguageIndex = 0;

                if (load == true)
                {
                    LanguageLoad(() =>
                    {
                        InitializeLanguage(callback, false);
                    });
                }
            }
        }

        internal static string GetSelectLanguageCode()
        {
            if (selectedLanguageIndex >= languages.Length)
            {
                return string.Empty;
            }

            return languages[selectedLanguageIndex];
        }

        private static int GetSelectLanguageIndex(string languageCode)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (languages[i].Equals(languageCode) == true)
                {
                    return i;
                }
            }

            return LANGUAGE_NOT_FOUND;
        }

        public static void OnGUI(System.Action callback)
        {
            if (languages != null)
            {
                EditorGUI.BeginChangeCheck();
                {
                    selectedLanguageIndex = Ui.PopupValue(selectedLanguageIndex, languages, UiStyle.ToolbarPopup, GUILayout.Width(80));
                }
                if (EditorGUI.EndChangeCheck() == true)
                {
                    string languageName = AssetManagementLanguage.GetSelectLanguageCode();
                    GpmMultilanguage.SelectLanguageByNativeName(
                        Constants.SERVICE_NAME,
                        languageName,
                        (result, resultMessage) =>
                        {
                            if (result == MultilanguageResultCode.SUCCESS)
                            {
                                Constants.LastLanguageName = languageName;

                                callback();
                            }
                            else
                            {
                                GpmLogger.Warn(
                                    string.Format("{0} (Code= {1})", Ui.GetString(Strings.KEY_CHANGE_LANGUAGE_ERROR_MESSAGE), result),
                                    Constants.SERVICE_NAME,
                                    typeof(AssetManagementLanguage));
                            }
                        });
                }
            }
        }
    }
}
