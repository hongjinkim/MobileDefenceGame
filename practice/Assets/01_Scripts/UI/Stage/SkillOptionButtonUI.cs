using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillOptionButtonUI : UIButton
{
    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillDescriptionText;

    public void Setup(SkillValue value)
    {
        skillNameText.text = value.Name; // Assuming ID is the skill name
        skillDescriptionText.text = value.Description; // Assuming Description is the skill description
    }

    protected override void OnClicked()
    {
       
    }
}
