using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutEvent : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private float Duration;

    public float StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
        return Duration;
    }


    private void Init()
    {
        CanvasGroup.alpha = 0;
    }


    private IEnumerator FadeOut()
    {
        Init();

        CanvasGroup.DOKill();
        Tweener tween = CanvasGroup.DOFade(1, Duration);
        yield return tween.WaitForCompletion();
    }

    public void DisableObject() => gameObject.SetActive(false);
}
