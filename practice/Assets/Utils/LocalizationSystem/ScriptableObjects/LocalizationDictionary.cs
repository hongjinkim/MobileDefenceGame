using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[CreateAssetMenu(menuName = "Localization/Dictionary", fileName = "LanguageDictionary")]
public class LocalizationDictionary : ScriptableObject
{
    public event Action OnUpdate;

    [field: SerializeField] public ELanguage CurrentLanguage { get; private set; } = ELanguage.Undefined;
    [SerializeField] public FontData[] fontData;

    public bool IsInitialized => CurrentLanguage != ELanguage.Undefined;


    private Dictionary<string, string> Dictionary = null;


    public void Initialize(Dictionary<string, string> dictionary, ELanguage language)
    {
        Dictionary = dictionary;
        CurrentLanguage = language;
        OnUpdate?.Invoke();
    }

    public string Translate(string key, string[] parameters = null)
    {
        if (Dictionary == null) return string.Empty;

        if (string.IsNullOrWhiteSpace(key) == true)
        {
            Debug.LogError($"<{GetType()}> Error. Request translate an empty key.");
            return string.Empty;
        }

        if (Dictionary.ContainsKey(key) == false)
        {
            Debug.LogError($"<{GetType()}> Error. Key not Found. - " + key);
            return string.Empty;
        }

        if (parameters == null) return Dictionary[key];
        else return string.Format(Dictionary[key], parameters);
    }

    public int GetMaterialIndex(TMP_FontAsset fontAsset, string materialName)
    {
        for(int i = 0; i < fontData.Length; i++)
        {
            if (fontAsset == fontData[i].FontAsset)
            {
                for(int j = 0; j < fontData[i].FontMaterials.Length; j++)
                {
                    if(materialName == fontData[i].FontMaterials[j].name) { return j; }
                }
            }
        }

        Debug.LogError("매칭되는 폰트or머티리얼가 없습니다");
        return 0; 
    }

    public TMP_FontAsset GetFontAsset(ELanguage language)
    {
        for (int i = 0; i < fontData.Length; i++)
        {
            if(language == fontData[i].Language)
            {
                return fontData[i].FontAsset;
                //fontMaterial = fontData[i].FontMaterials[materialIndex];
            }
        }

        Debug.LogError("매칭되는 폰트가 없습니다");
        return fontData[0].FontAsset;
    }

    public Material GetFontMaterial(ELanguage language, int materialIndex)
    {
        for (int i = 0; i < fontData.Length; i++)
        {
            if (language == fontData[i].Language)
            {
                return fontData[i].FontMaterials[materialIndex];
            }
        }

        Debug.LogError("매칭되는 머티리얼이 없습니다");
        return fontData[0].FontMaterials[0];
    }
}
