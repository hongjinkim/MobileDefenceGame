using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_Text))]
public abstract class LocalizedTMPBase : MonoBehaviour
{
    public LocalizationDictionary Dictionary;

    protected TMP_Text TMP
    => tmp == null
        ? (tmp = GetComponent<TMP_Text>())
        : tmp;

    private TMP_Text tmp = null;

    private ELanguage textLanguage = ELanguage.Korean; //모든 기본값은 한국어
    private int MaterialIndex;
    private bool hasMaterialInfo;

    private void Awake()
    {
        GetMaterialInfo();
    }

    protected virtual void OnEnable()
    {
        Dictionary.OnUpdate += UpdateText;

        try
        {
            UpdateText();
        }
        catch
        {
            TMP.SetText(string.Empty);
        }
    }

    protected virtual void OnDisable()
    {
        Dictionary.OnUpdate -= UpdateText;
    }

    protected virtual void LogKeyEmptyError(string typeString)
    {
        var hierarchyPath = $"[{transform.GetSiblingIndex():0}] {gameObject.name}";
        var parent = transform;
        while (parent.parent != null)
        {
            parent = parent.parent;
            hierarchyPath = $"[{parent.GetSiblingIndex():0}] {parent.gameObject.name} / {hierarchyPath}";
        }

        Debug.Log($"[!] <{typeString}> Error. Localization Key(or Format) is Empty. ( {hierarchyPath} )");
    }

    void GetMaterialInfo()
    {
        if (!hasMaterialInfo)
        {
            hasMaterialInfo = true;
            MaterialIndex = Dictionary.GetMaterialIndex(TMP.font, TMP.fontMaterial.name.Replace(" (Instance)", ""));
        }
    }

    public virtual void UpdateText() 
    {
        GetMaterialInfo();

        if (textLanguage != Dictionary.CurrentLanguage)
        {
            textLanguage = Dictionary.CurrentLanguage;
            TMP.font = Dictionary.GetFontAsset(textLanguage);
            TMP.fontMaterial = Dictionary.GetFontMaterial(textLanguage, MaterialIndex);
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        try
        {
            if (Dictionary == null)
            {
                string path = "Assets/POOMLibrary/LocalizationSystem/LanguageDictionary.asset";
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