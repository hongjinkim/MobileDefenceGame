using System;
using UnityEngine;



[CreateAssetMenu(menuName = "Localization/System", fileName = "LocalizationSystem")]
public class LocalizationSystem : ScriptableObject
{
    public event Action<ELanguage> OnUpdatedLanguage;

    [SerializeField] private LocalizationConfiguration Configuration;
    [SerializeField] private LocalizationLanguageDataLibrary DataLibrary;
    [SerializeField] private LocalizationFontDataLibrary FontLibrary;

    [Header("Handlers")]
    [SerializeField] private LocalizationDictionary Dictionary;
    [SerializeField] private LocalizationFontCollection FontCollection;

    public LocalizationDictionary CurrentDictionary => Dictionary;

    public ELanguage CurrentLanguage => Dictionary.CurrentLanguage;


    public void Initialize()
    {
        DataLibrary.Initialize();

        var initialLanguage = Configuration.GetInitializeLanguage(this);
        UpdateLanguage(initialLanguage);
    }
        
    public void UpdateLanguage(ELanguage language)
    {
        var fontData = FontLibrary.Get(language);
        FontCollection.Set(fontData);
            
        var dictionary = DataLibrary.Get(language);
        Dictionary.Initialize(dictionary, language); 

        OnUpdatedLanguage?.Invoke(language); 
    }

    public bool GetLanguageInit()
    {
        return PlayerPrefs.GetInt("Kitty_LanguageSet") == 1;
    }

    public void SetLanguageInit()
    {
        PlayerPrefs.SetInt("Kitty_LanguageSet", 1);
    }


    public ELanguage GetCurrentLanguage()
    {
        ELanguage lang = PlayerPrefs.GetString("Kitty_Language").ToPoomLanguage();

        if (lang == ELanguage.Other)
            return Configuration.FallbackLanguage;
        else
            return lang;
    }


    public void SetCurrentLanguage(ELanguage currentLanguage)
    {
        PlayerPrefs.SetString("Kitty_Language", currentLanguage.ToString());
    }

    public string getTextWithCustomLanguage(ELanguage language, string key)
    {
        var dictionary = DataLibrary.Get(language);
        return dictionary[key];
    }
}
