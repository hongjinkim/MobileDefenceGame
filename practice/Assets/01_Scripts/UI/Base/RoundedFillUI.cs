using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundedFillUI : MonoBehaviour
{
    public RectTransform BaseRectTransform;
    public RectTransform BackLeft;
    public RectTransform BackCenter;
    public RectTransform BackRight;
    public RectTransform Range;
    public Image Left;
    public Image Center;
    public Image Right;

    public Vector2 PaddingMin = new Vector2(2, 2);
    public Vector2 PaddingMax = new Vector2(-2, -2);

    [SerializeField, Range(0f, 1f)]
    private float halfCirclePossession = 0.07f;

    [SerializeField, Range(0f, 1f)]
    private float progress = 1f;

    private RectTransform leftRectTransform;
    private RectTransform centerRectTransform;
    private RectTransform rightRectTransform;

    private const float AFTER_TIME_SLIDE = 0.6f;
    private bool isAfterSliding = false;
    private float targetValue;
    private float delta;
    private bool isSet = false;

    private void Awake()
    {
        BasicSetting();
    }

    private void Update()
    {
        AfterSlide();
    }


    public void SetSize(Vector2 size)
    {
        BaseRectTransform.sizeDelta = size;

        float radius = size.y * 0.5f;
        BackLeft.anchorMin = new Vector2(0, 0);
        BackLeft.anchorMax = new Vector2(0, 1);
        BackLeft.offsetMin = new Vector2(0, 0);
        BackLeft.offsetMax = new Vector2(0, 0);
        BackLeft.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius);

        BackCenter.anchorMin = new Vector2(0, 0);
        BackCenter.anchorMax = new Vector2(1, 1);
        BackCenter.offsetMin = new Vector2(radius, 0);
        BackCenter.offsetMax = new Vector2(-radius, 0);

        BackRight.anchorMin = new Vector2(1, 0);
        BackRight.anchorMax = new Vector2(1, 1);
        BackRight.offsetMin = new Vector2(0, 0);
        BackRight.offsetMax = new Vector2(0, 0);
        BackRight.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius);

        Range.anchorMin = new Vector2(0, 0);
        Range.anchorMax = new Vector2(1, 1);
        Range.offsetMin = PaddingMin;
        Range.offsetMax = PaddingMax;

        SetArea(new Vector2(size.x - PaddingMin.x + PaddingMax.x, size.y - PaddingMin.y + PaddingMax.y));
    }

    private void SetArea(Vector2 size)
    {
        float radius = size.y * 0.5f;

        Range.anchorMin = new Vector2(0, 0.5f);
        Range.anchorMax = new Vector2(1, 0.5f);
        Range.offsetMin = new Vector2(radius, 0);
        Range.offsetMax = new Vector2(-radius, 0);

        leftRectTransform.sizeDelta = new Vector2(radius, 2f * radius);

        centerRectTransform.anchorMin = new Vector2(0, 0.5f);
        centerRectTransform.anchorMax = new Vector2(1, 0.5f);
        centerRectTransform.offsetMin = new Vector2(0, 0);
        centerRectTransform.offsetMax = new Vector2(0, 0);
        centerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius * 2f);

        rightRectTransform.sizeDelta = new Vector2(radius, 2f * radius);

        float width = size.x - size.y;
        float numerator = Mathf.PI * radius;
        float denominator = 2 * width - (4 - Mathf.PI) * radius;
        halfCirclePossession = (numerator / denominator) * 0.5f;

        SetProgress(progress);
    }


    public void SetProgress(float value)
    {
        value = Mathf.Clamp01(value);
        progress = value;

        if (value < halfCirclePossession * 2f)
        {
            Center.fillAmount = 0f;
            float remapped = value / (halfCirclePossession * 2f);
            Left.fillAmount = remapped;
            Right.fillAmount = remapped;
            float x = leftRectTransform.sizeDelta.x + rightRectTransform.sizeDelta.x;
            rightRectTransform.anchoredPosition = new Vector2(x * (remapped - 1), 0);
        }
        else
        {
            Left.fillAmount = 1f;
            Right.fillAmount = 1f;

            float rest = Mathf.Clamp01((value - halfCirclePossession * 2f) / (1f - halfCirclePossession * 2f));
            Center.fillAmount = rest;
            rightRectTransform.anchoredPosition = new Vector2(Range.rect.width * rest, 0);
        }
    }

    void AfterSlide()
    {
        if (!isAfterSliding) { return; }

        if (targetValue < progress)
            SetProgress(progress - delta * Time.deltaTime);

        else
        {
            SetProgress(targetValue);
            isAfterSliding = false;
        }
    }

    public void SetProgress_After(float value)
    {
        //목표값이 현재값보다 높은경우 (ex힐등으로 체력회복)
        if (value > progress)
        {
            isAfterSliding = false; //afterslide중이었다면 종료시킴
            SetProgress(value); //타겟값으로 바로 지정
        }

        else
        {
            isAfterSliding = true;
            targetValue = value;
            delta = (progress - targetValue) / AFTER_TIME_SLIDE;
        }
    }

    public void StopAfterSlide()
    {
        isAfterSliding = false;
    }

    public void BasicSetting()
    {
        if (!isSet)
        {
            isSet = true;
            leftRectTransform = (RectTransform)Left.transform;
            rightRectTransform = (RectTransform)Right.transform;
            centerRectTransform = (RectTransform)Center.transform;
        }
    }


#if UNITY_EDITOR
    [Header("UNITY EDITOR ONLY")]
    [Range(0f, 1f)]
    public float TestValue = 1f;

    public Vector2 TestSize = new Vector2(150, 30);

    private void Reset()
    {
        if (BaseRectTransform == null)
        {
            BaseRectTransform = GetComponent<RectTransform>();
        }
    }

    private void OnValidate()
    {
        if (Range == null || Left == null || Center == null || Right == null)
        {
            return;
        }

        CacheRectTransforms();
        SetProgress(TestValue);
    }

    private void CacheRectTransforms()
    {
        if (leftRectTransform == null) { leftRectTransform = (RectTransform)Left.transform; }
        if (rightRectTransform == null) { rightRectTransform = (RectTransform)Right.transform; }
        if (centerRectTransform == null) { centerRectTransform = (RectTransform)Center.transform; }
    }

    [ContextMenu("Resize by Test Size")]
    public void Test()
    {
        SetSize(TestSize);
    }
#endif

}