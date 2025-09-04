using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ELanguage
{
    Undefined,
    English,
    Korean,
    ChineseSimplified,
    ChineseTraditional,
    Japanese,
    Spanish,
    French,
    German,
    Portuguese,
    Italian,
    Other
}

public static class PoomLanguageExtensions
{
    public static SystemLanguage ToSystemLanguage(this ELanguage language)
    {
        return language switch
        {
            ELanguage.Undefined => SystemLanguage.Unknown,
            ELanguage.English => SystemLanguage.English,
            ELanguage.Korean => SystemLanguage.Korean,
            ELanguage.ChineseSimplified => SystemLanguage.ChineseSimplified,
            ELanguage.ChineseTraditional => SystemLanguage.ChineseTraditional,
            ELanguage.Japanese => SystemLanguage.Japanese,
            ELanguage.Spanish => SystemLanguage.Spanish,
            ELanguage.French => SystemLanguage.French,
            ELanguage.German => SystemLanguage.German,
            ELanguage.Portuguese => SystemLanguage.Portuguese,
            ELanguage.Italian => SystemLanguage.Italian,
            ELanguage.Other => SystemLanguage.Unknown,
            _ => SystemLanguage.Unknown
        };
    }

    public static ELanguage ToPoomLanguage(this SystemLanguage language)
    {
        return language switch
        {
            SystemLanguage.Unknown => ELanguage.Undefined,
            SystemLanguage.English => ELanguage.English,
            SystemLanguage.Korean => ELanguage.Korean,
            SystemLanguage.ChineseSimplified => ELanguage.ChineseSimplified,
            SystemLanguage.ChineseTraditional => ELanguage.ChineseTraditional,
            SystemLanguage.Japanese => ELanguage.Japanese,
            SystemLanguage.Spanish => ELanguage.Spanish,
            SystemLanguage.French => ELanguage.French,
            SystemLanguage.German => ELanguage.German,
            SystemLanguage.Portuguese => ELanguage.Portuguese,
            SystemLanguage.Italian => ELanguage.Italian,
            _ => ELanguage.Other
        };
    }

    public static ELanguage ToPoomLanguage(this string language)
    {
        return language switch
        {
            "Korean" => ELanguage.Korean,
            "English" => ELanguage.English,
            "ChineseSimplified" => ELanguage.ChineseSimplified,
            "ChineseTraditional" => ELanguage.ChineseTraditional,
            "Japanese" => ELanguage.Japanese,
            "Spanish" => ELanguage.Spanish,
            "French" => ELanguage.French,
            "German" => ELanguage.German,
            "Portuguese" => ELanguage.Portuguese,
            "Italian" => ELanguage.Italian,

            _ => ELanguage.Other
        };
    }
}