using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceUI : MonoBehaviour
{
    public List<Button> optionButtons; 
    public List<Text> optionLabels;   
    private System.Action<SkillUpgradeValue> onChosenCallback;
    private List<SkillUpgradeValue> currentChoices;

    private void Start()
    {
        Hide();
    }

    public void Show(List<SkillUpgradeValue> choices, System.Action<SkillUpgradeValue> onChosen)
    {
        gameObject.SetActive(true);
        currentChoices = choices;
        onChosenCallback = onChosen;

        for (int i = 0; i < optionButtons.Count; i++)
        {
            if (i < choices.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionLabels[i].text = choices[i].Description;
                int idx = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnClicked(idx));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnClicked(int idx)
    {
        onChosenCallback?.Invoke(currentChoices[idx]);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
