using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Localization/FontDataLibrary", fileName = "LocalizationFontDataLibrary")]
public class LocalizationFontDataLibrary : ScriptableObject
{
    [SerializeField]
    private List<LanguageFontData> List = new();


    public LanguageFontData Get(ELanguage language) 
        => List.First(_ => _.Language == language);

}


