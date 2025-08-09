using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceUI : MonoBehaviour
{
    public List<SkillOptionButtonUI> optionButtons; 
    private Action<SkillUpgradeValue> onChosenCallback;

    private void Start()
    {
        Hide();
    }

    public void Show(List<SkillUpgradeValue> choices, Action<SkillUpgradeValue> onChosen)
    {
        this.onChosenCallback = onChosen;
        gameObject.SetActive(true);

        for (int i = 0; i < optionButtons.Count; i++)
        {
            var btn = optionButtons[i];

            btn.onClick.RemoveAllListeners();

            if (i < choices.Count)
            {
                var choice = choices[i];  // 로컬에 캡처 (for-closure 버그 방지)
                btn.Setup(choice);
                btn.onClick.AddListener(() => OnOptionSelected(choice));
            }
        }
    }

    private void OnOptionSelected(SkillUpgradeValue chosenUpgrade)
    {
        onChosenCallback?.Invoke(chosenUpgrade);
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
