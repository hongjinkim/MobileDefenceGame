using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlinkEvent : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float blinkInterval = 1.0f; // 깜빡임 간격 (초)

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
            // 부드러운 사라짐
            canvasGroup.DOFade(0.0f, blinkInterval / 2.0f);
            yield return new WaitForSeconds(blinkInterval / 2.0f);

            // 부드러운 나타남
            canvasGroup.DOFade(1.0f, blinkInterval / 2.0f);
            yield return new WaitForSeconds(blinkInterval / 1.5f);
        }
    }
}
