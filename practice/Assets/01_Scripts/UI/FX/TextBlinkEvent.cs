using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlinkEvent : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float blinkInterval = 1.0f; // ������ ���� (��)

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        canvasGroup.alpha = 0;
        while (true)
        {
            // �ε巯�� �����
            canvasGroup.DOFade(0.0f, blinkInterval / 2.0f);
            yield return new WaitForSeconds(blinkInterval / 2.0f);

            // �ε巯�� ��Ÿ��
            canvasGroup.DOFade(1.0f, blinkInterval / 2.0f);
            yield return new WaitForSeconds(blinkInterval / 1.5f);
        }
    }
}
