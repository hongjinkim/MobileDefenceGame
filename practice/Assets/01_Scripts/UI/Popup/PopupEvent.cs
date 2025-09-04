using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PopupEvent : PopupEventBase
{
    [Serializable]
    public struct TweenSettings
    {
        public Vector3 StartScale;
        public Vector3 EndScale;
        public Ease EaseType;
        public bool UseFade;
        public float Duration;
    }

    [SerializeField] protected TweenSettings OpenTween = default;
    [SerializeField] protected TweenSettings CloseTween = default;

    private Coroutine openRescaling = null;
    private Coroutine openAppearing = null;


    public override void Open()
    {
        //Debug.Log("PopupEvent.Open()", gameObject);

        StopAllCoroutines();
        CanvasGroup.interactable = true;
        //if (RectTransform == null) { Init(); }

        if (gameObject.activeInHierarchy == false) return;

        RectTransform.transform.localScale = OpenTween.StartScale;

        if (gameObject.activeInHierarchy == true)
        {
            if (openRescaling != null)
            {
                StopCoroutine(openRescaling);
            }
            openRescaling = StartCoroutine(Rescale());

            if (OpenTween.UseFade == true)
            {
                if (openAppearing != null)
                {
                    StopCoroutine(openAppearing);
                }
                openAppearing = StartCoroutine(Appear());
            }
        }
        else
        {
            RectTransform.localScale = OpenTween.EndScale;
            CanvasGroup.alpha = 1f;
        }
    }

    private IEnumerator Rescale()
    {
        if (OpenTween.Duration > 0)
        {
            RectTransform
                .DOScale(OpenTween.EndScale, OpenTween.Duration)
                .SetEase(OpenTween.EaseType)
                .From(OpenTween.StartScale); // 시작 스케일 지정

            // 트윈이 끝날 때까지 기다리기
            yield return new WaitForSeconds(OpenTween.Duration);
        }
        else
        {
            RectTransform.localScale = OpenTween.EndScale;
        }

        openRescaling = null;

        yield return StartCoroutine(AutoClose());
    }

    private IEnumerator Appear()
    {
        //Debug.LogError("Appear");
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / OpenTween.Duration;
            CanvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
        openAppearing = null;
    }

    public override void Close()
    {
        base.Close();
        CanvasGroup.interactable = false;
        RectTransform.transform.localScale = CloseTween.EndScale;
        RectTransform
            .DOScale(CloseTween.StartScale, CloseTween.Duration)
            .SetEase(CloseTween.EaseType)
            .OnComplete(PushPopup)
            .SetUpdate(true);

        if (CloseTween.UseFade)
        {
            CanvasGroup.alpha = 1f;
            CanvasGroup.DOFade(0f, CloseTween.Duration).SetUpdate(true);
        }
    }

    private void PushPopup() => PopupManager.Instance.PopupClose();
}
