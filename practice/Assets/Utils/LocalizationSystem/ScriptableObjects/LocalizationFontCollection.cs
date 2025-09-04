using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


[CreateAssetMenu(menuName = "Localization/FontCollection", fileName = "LanguageFontCollection")]
public class LocalizationFontCollection : ScriptableObject
{
    public event Action OnChanged;


    [field: SerializeField]
    public ELanguage CurrentLanguage { get; private set; } = ELanguage.Undefined;

                
    private TMP_FontAsset FallbackFont;
    private Material FallbackMaterial;
    private List<FontMaterialSet> List = new();


    public (TMP_FontAsset font, Material material) Get(string key)
    {
        if (string.IsNullOrWhiteSpace(key) == true)
        {
            return (FallbackFont, FallbackMaterial);
        }

        var data = List.FirstOrDefault(_ => _.Key.Equals(key));
        return data == null ? (FallbackFont, FallbackMaterial) : (data.Font, data.Material);
    }

    public void Set(LanguageFontData data)
    {
        FallbackFont = data.FallbackFontAsset; 
        FallbackMaterial = data.FallbackMaterial;
        List = data.KeyFontSet.ToList();
        CurrentLanguage = data.Language;

        OnChanged?.Invoke();
    }

}


