using System;
using UnityEngine;
using TMPro;


[CreateAssetMenu(menuName = "Localization/FontData", fileName = "FontData")]
public class FontData: ScriptableObject
{
    public ELanguage Language;
    public TMP_FontAsset FontAsset;
    public Material[] FontMaterials;
}

