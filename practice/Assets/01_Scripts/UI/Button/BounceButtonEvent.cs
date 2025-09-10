using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// ��¦ Ŀ���ٰ� �۾����� ȿ��
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
        yield return null; //UI�⺻ pivot���� ��, ������ ������ pivot ���� 

        RectTransform rect = GetComponent<RectTransform>();
        Vector2 centerPivot = new Vector2(0.5f, 0.5f);

        //�߾�pivot������ �ϰ����ͷ� ������
        if (pivotCenter && rect.pivot != centerPivot)
        {
            Vector3 deltaPosition = rect.pivot - centerPivot;
            deltaPosition.Scale(rect.rect.size);
            deltaPosition.Scale(rect.localScale);
            deltaPosition = rect.transform.localRotation * deltaPosition;

            rect.pivot = centerPivot;
            rect.localPosition -= deltaPosition;
        }

        //�⺻ scale���� ���
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
