using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Localization/LanguageDataLibrary", fileName = "LocalizationDataLibrary")]
public class LocalizationLanguageDataLibrary : ScriptableObject
{
    private readonly Dictionary<ELanguage, Dictionary<string, string>> CacheDic = new();


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

        // 파싱 해 오는 데이터 형식에 맞춰서 데이터 채워 넣기
    }

}