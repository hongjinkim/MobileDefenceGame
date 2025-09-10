using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 살짝 커졌다가 작아지는 효과
public class BounceButtonEvent : UIButton
{
    public Ease SelectEase = Ease.OutQuad;
    public float AnimTime = 0.1f;
    public Vector3 TargetScale = new Vector3(1.1f, 1.1f, 1.1f);
    public bool Loop = true;
    public bool pivotCenter = true;

    private Vector3 oriScale;
    private Vector3 targetWeightedScale;


    private IEnumerator Start()
    {
        yield return null; //UI기본 pivot설정 후, 안전한 시점에 pivot 변경 

        RectTransform rect = GetComponent<RectTransform>();
        Vector2 centerPivot = new Vector2(0.5f, 0.5f);

        //중앙pivot설정시 일괄센터로 세팅함
        if (pivotCenter && rect.pivot != centerPivot)
        {
            Vector3 deltaPosition = rect.pivot - centerPivot;
            deltaPosition.Scale(rect.rect.size);
            deltaPosition.Scale(rect.localScale);
            deltaPosition = rect.transform.localRotation * deltaPosition;

            rect.pivot = centerPivot;
            rect.localPosition -= deltaPosition;
        }

        //기본 scale값을 등록
        oriScale = transform.localScale;
        //Debug.LogError(this.name + " / " + oriScale);
        targetWeightedScale = new Vector3(oriScale.x * TargetScale.x, oriScale.y * TargetScale.y, oriScale.z * TargetScale.z);
    }

    protected override void OnClicked()
    {
        transform.localScale = oriScale;
        transform.DORewind();
        if (Loop == true)
            transform.DOScale(targetWeightedScale, AnimTime).SetEase(SelectEase).SetLoops(2, LoopType.Yoyo);
        else
            transform.DOScale(targetWeightedScale, AnimTime).SetEase(SelectEase);
    }

}
