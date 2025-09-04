using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedTMPFontController : MonoBehaviour
{
    [SerializeField]
    private LocalizationFontCollection FontCollection = null;

    [SerializeField]
    private string Key;


    private TMP_Text TMP
        => tmp == null
        ? (tmp = GetComponent<TMP_Text>())
        : tmp;


    private ELanguage currentLanguage = ELanguage.Undefined;
    private TMP_Text tmp;


    private void OnEnable()
    {
        FontCollection.OnChanged += UpdateFont;
        UpdateFont();
    }

    private void OnDisable()
    {
        FontCollection.OnChanged -= UpdateFont;
    }


    public void UpdateKey(string key)
    {
        Key = key;
        UpdateFont();
    }


    private void UpdateFont()
    {
        var fontCollectionLanguage = FontCollection.CurrentLanguage;

        if (fontCollectionLanguage == ELanguage.Undefined) return;

        if (currentLanguage != fontCollectionLanguage)
        {
            var (font, material) = FontCollection.Get(Key);

            TMP.font = font;
            TMP.fontSharedMaterial = material;

            currentLanguage = fontCollectionLanguage;
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        try
        {
            if (FontCollection == null)
            {
                string path = "Assets/Utils/LocalizationSystem/LanguageFontCollection.asset";
                var collection = UnityEditor.AssetDatabase.LoadAssetAtPath<LocalizationFontCollection>(path);
                FontCollection = collection;
            }
        }
        catch
        {

        }
    }
#endif

}

