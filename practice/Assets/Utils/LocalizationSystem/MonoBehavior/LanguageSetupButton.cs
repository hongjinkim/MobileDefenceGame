using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LanguageSetupButton : MonoBehaviour
{
    public event Action<bool> OnSelected;

    public LocalizationSystem System;
    public ELanguage TargetLanguage = ELanguage.Undefined;

        
    private Button Button
        => button == null
        ? (button = GetComponent<Button>())
        : button;

    private Button button;
    private void Start()
    {
        SetButtonColor(System.GetCurrentLanguage());
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(Click);
        System.OnUpdatedLanguage += OnChangedLanguage;
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(Click);
        System.OnUpdatedLanguage -= OnChangedLanguage;
    }


    private void Click()
    {
        if (System == null)
        {
            Debug.LogError($"<{GetType()}> Error. LocalizationSystem 애셋을 연결해주세요.");
            return;
        }

        if (TargetLanguage == ELanguage.Undefined)
        {
            Debug.Log($"<{GetType()}> Error. 타겟 언어가 Undefined입니다.");
            return;
        }

        if (System.CurrentLanguage == TargetLanguage)
        {
            Debug.Log($"<{GetType()}> 이미 현재 언어({TargetLanguage})로 설정되어 있습니다.");
            return;
        }

        System.UpdateLanguage(TargetLanguage);
    }

    private void OnChangedLanguage(ELanguage language)
    {
        SetButtonColor(language);
        OnSelected?.Invoke(language == TargetLanguage);
    }

    private void SetButtonColor(ELanguage language)
    {
        GetComponent<Image>().color = language == TargetLanguage ? Color.gray : Color.white;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        try
        {
            if (System == null)
            {
                string path = "Assets/POOMLibrary/LocalizationSystem/LocalizationSystem.asset";
                var system = UnityEditor.AssetDatabase.LoadAssetAtPath<LocalizationSystem>(path);
                System = system;
            }
        }
        catch
        {

        }
    }
#endif
}

