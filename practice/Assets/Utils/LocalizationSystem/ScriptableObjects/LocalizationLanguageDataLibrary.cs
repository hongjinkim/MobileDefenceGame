using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


[CreateAssetMenu(menuName = "Localization/LanguageDataLibrary", fileName = "LocalizationDataLibrary")]
public class LocalizationLanguageDataLibrary : ScriptableObject
{
    public Dictionary<ELanguage, Dictionary<string, string>> CacheDic = new();


    public Dictionary<string, string> Get(ELanguage targetLanguage)
    {
        if (CacheDic.ContainsKey(targetLanguage) == false)
        {
            Debug.LogError($"<{GetType()}> Error. 존재하지 않는 언어데이터를 요청함. {targetLanguage}");
            return null;
        }

        return CacheDic[targetLanguage];
    }

    public void Initialize()
    {
        CacheDic.Clear();

        var localizationList = DataTable.Localization.GetList();

        foreach (var loc in localizationList)
        {
            // Localization 클래스의 Public 필드 가져오기
            var fields = typeof(DataTable.Localization)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.Name == "Key")
                    continue; // Key는 번역값 아님

                string langName = field.Name;                  // "Korean", "English"
                ELanguage lang = langName.ToEnumLanguage(); // ELanguage.Korean, ELanguage.English
                string value = field.GetValue(loc)?.ToString();

                if (CacheDic.ContainsKey(lang) == false)
                    CacheDic[lang] = new Dictionary<string, string>();

                CacheDic[lang][loc.Key] = value;
            }
        }
    }

}
