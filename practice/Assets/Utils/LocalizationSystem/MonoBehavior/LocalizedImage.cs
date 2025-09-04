using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class LocalizedImage : MonoBehaviour
{
    public LocalizationDictionary Dictionary;
    private Image targetImage;
        
    [SerializeField] LocalizedImageData localizedSprite;

    private void Awake()
    {
        targetImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Dictionary.OnUpdate += SetImage;
        SetImage();
    }
    private void OnDisable()
    {
        Dictionary.OnUpdate -= SetImage;
    }

    void SetImage()
    {
        switch (Dictionary.CurrentLanguage)
        {
            case ELanguage.Korean: targetImage.sprite = localizedSprite.Korean_Image; break;
            case ELanguage.English: targetImage.sprite = localizedSprite.English_Image; break;
            case ELanguage.ChineseSimplified: targetImage.sprite = localizedSprite.ChineseSimplified_Image; break;
            case ELanguage.ChineseTraditional: targetImage.sprite = localizedSprite.ChineseTraditional_Image; break;
            case ELanguage.Japanese: targetImage.sprite = localizedSprite.Japanese_Image; break;
            case ELanguage.Spanish: targetImage.sprite = localizedSprite.Spanish_Image; break;
            case ELanguage.French: targetImage.sprite = localizedSprite.French_Image; break;
            case ELanguage.German: targetImage.sprite = localizedSprite.German_Image; break;
            case ELanguage.Portuguese: targetImage.sprite = localizedSprite.Portuguese_Image; break;
            case ELanguage.Italian: targetImage.sprite = localizedSprite.Italian_Image; break;

            default:
                Debug.LogError("Localized Image설정에 Error발생");
                targetImage.sprite = localizedSprite.Korean_Image; break;
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        try
        {
            if (Dictionary == null)
            {
                string path = "Assets/Utils/LocalizationSystem/LanguageDictionary.asset";
                var dic = UnityEditor.AssetDatabase.LoadAssetAtPath<LocalizationDictionary>(path);
                Dictionary = dic;
            }
        }
        catch
        {

        }
    }
#endif
}
