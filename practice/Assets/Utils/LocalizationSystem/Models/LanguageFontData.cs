using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class LanguageFontData
{
    public ELanguage Language = ELanguage.Undefined;
    public TMP_FontAsset FallbackFontAsset = null;
    public Material FallbackMaterial = null;
    public List<FontMaterialSet> KeyFontSet = new List<FontMaterialSet>();
}


