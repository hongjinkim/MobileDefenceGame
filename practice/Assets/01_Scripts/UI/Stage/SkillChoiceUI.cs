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
            if (i < choices.Count)
            {
                SkillUpgradeValue currentChoice = choices[i];
                SkillOptionButtonUI button = optionButtons[i];

                button.Setup(currentChoice);
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => OnOptionSelected(currentChoice));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
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
