using UnityEngine;


[CreateAssetMenu(menuName = "Localization/Configuration", fileName = "LocalizationConfiguration")]
public class LocalizationConfiguration : ScriptableObject
{
    public ELanguageSettings DefaultLanguage = ELanguageSettings.SYSTEM_LANGUAGE;
    public ELanguage FallbackLanguage = ELanguage.English;
    public ELanguage[] LanguageAvailable;
    private LocalizationSystem localizationSystem;

    public ELanguage GetInitializeLanguage(LocalizationSystem system)
    {
        localizationSystem = system;

        if (localizationSystem.GetLanguageInit()) 
            return localizationSystem.GetCurrentLanguage();

        ELanguage lang;

        if (DefaultLanguage == ELanguageSettings.SYSTEM_LANGUAGE)
        {
            ELanguage systemLanguage = Application.systemLanguage.ToPoomLanguage();

            bool isAvailableLanguage = false;
            for (int i = 0; i < LanguageAvailable.Length; i++)
            {
                if (LanguageAvailable[i] == systemLanguage)
                {
                    isAvailableLanguage = true;
                    break;
                }
            }

            if (isAvailableLanguage)
                lang = systemLanguage;
            else
                lang = FallbackLanguage;
        }

        else
            lang = FallbackLanguage;

        localizationSystem.SetLanguageInit();
        localizationSystem.SetCurrentLanguage(lang);
        return lang;
    }
}

public enum ELanguageSettings
{
    SYSTEM_LANGUAGE,
    FALLBACK_LANGUAGE,
}

