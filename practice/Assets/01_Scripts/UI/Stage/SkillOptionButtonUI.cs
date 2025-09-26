using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillOptionButtonUI : UIButton
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI skillDescriptionText;
    [SerializeField] Image iconImage;

    [Header("FX")]
    public PopupEvent popupEvent;

    public void Setup(SkillValue value)
    {
        skillNameText.text = value.Name; // Assuming ID is the skill name
        skillDescriptionText.text = value.Description; // Assuming Description is the skill description
        //iconImage.sprite = 이름 바탕으로 딕셔너리 생성 후 스파라이트 가져오기
    }

    protected override void OnClicked()
    {
       
    }
}
