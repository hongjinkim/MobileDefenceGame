using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;

[DisallowMultipleComponent]
public class CircleTransition : MonoBehaviour
{
    [Header("Overlay Image (full-screen panel)")]
    [SerializeField] private Image overlayImage;        // 전체 화면 덮는 Image (Material= UI/CircleWipePixel)
    [SerializeField] private string radiusProp = "_Radius";
    [SerializeField] private string centerProp = "_Center";
    [SerializeField] private string featherProp = "_Feather";
    [SerializeField] private string rectProp = "_RectSize";
    [SerializeField] private string colorProp = "_Color";

    [Header("Timing")]
    [SerializeField] private float closeDuration = 0.35f;
    [SerializeField] private float darkHold = 0.15f;
    [SerializeField] private float openDuration = 0.45f;

    [Header("Circle Params (normalized by rect height)")]
    [SerializeField] private float startRadius = 1.5f;     // 화면 밖에서 시작
    [SerializeField] private float endRadius = 0.00f;    // 거의 완전 닫힘
    [SerializeField] private float feather = 0.20f;
    [SerializeField] private Color overlayColor = new(0, 0, 0, 0.85f);
    [SerializeField, Tooltip("Rect 기준 0~1")] private Vector2 center01 = new(0.5f, 0.5f);

    [Header("Toggle at Dark (optional)")]
    [SerializeField] private List<GameObject> enableOnDark = new();   // 어두울 때 켤 패널들
    [SerializeField] private List<GameObject> disableOnDark = new();  // 어두울 때 끌 패널들

    [Header("Input Blocking")]
    [SerializeField] private bool blockPointerDuringTransition = true;         // 마우스/터치
    [SerializeField] private bool blockNavigationDuringTransition = true;      // 키보드/패드 Submit/Cancel 등
    [SerializeField] private EventSystem eventSystem;                           // 선택: 지정 없으면 EventSystem.current 사용

    [Header("Task")]
    private Coroutine _waitRoutine;
    private CancellationTokenSource _cts;

    Material mat;
    int _radiusId, _centerId, _featherId, _rectId, _colorId;
    Tweener tweenClose, tweenOpen, tweenDarkHold;

    // 복원용 저장
    bool _prevRaycastTarget;
    bool _prevSendNavigationEvents;

    void Awake()
    {
        if (!overlayImage)
        {
            Debug.LogError("[CircleTransition] overlayImage가 필요합니다.", this);
            enabled = false;
            return;
        }

        // 머티리얼 인스턴스화 (공유 방지)
        mat = Instantiate(overlayImage.material);
        overlayImage.material = mat;

        _radiusId = Shader.PropertyToID(radiusProp);
        _centerId = Shader.PropertyToID(centerProp);
        _featherId = Shader.PropertyToID(featherProp);
        _rectId = Shader.PropertyToID(rectProp);
        _colorId = Shader.PropertyToID(colorProp);

        ApplyStaticParams();
        overlayImage.enabled = false;

        // 입력 차단 기본값 팁: Overlay Image에는 스프라이트 없음(단색) 추천
        // (스프라이트 알파 히트 테스트를 사용하면 구멍 부분이 통과될 수 있어요)
    }

    void OnDestroy()
    {
        tweenClose?.Kill();
        tweenOpen?.Kill();
        if (mat) Destroy(mat);
    }

    void OnRectTransformDimensionsChange()
    {
        if (overlayImage && mat) UpdateRectSize();
    }

    void ApplyStaticParams()
    {
        UpdateRectSize();
        mat.SetVector(_centerId, new Vector4(center01.x, center01.y, 0, 0));
        mat.SetFloat(_featherId, feather);
        mat.SetColor(_colorId, overlayColor);
        mat.SetFloat(_radiusId, startRadius);
    }

    void UpdateRectSize()
    {
        var rt = overlayImage.rectTransform;
        var size = rt.rect.size; // px
        mat.SetVector(_rectId, new Vector4(size.x, size.y, 0, 0));
    }

    void ToggleAtDark()
    {
        foreach (var go in disableOnDark)
            if (go) go.SetActive(false);

        foreach (var go in enableOnDark)
            if (go) go.SetActive(true);
    }

    // ---------- 입력 차단 ----------
    void BeginBlockInput()
    {
        // 포인터(클릭/터치) 차단: 오버레이가 레이캐스트를 흡수하게
        if (blockPointerDuringTransition)
        {
            _prevRaycastTarget = overlayImage.raycastTarget;
            overlayImage.raycastTarget = true; // 전체 Rect가 클릭 흡수
        }

        // 키보드/패드 네비게이션 차단
        if (blockNavigationDuringTransition)
        {
            var es = eventSystem != null ? eventSystem : EventSystem.current;
            if (es != null)
            {
                _prevSendNavigationEvents = es.sendNavigationEvents;
                es.sendNavigationEvents = false; // Submit/Cancel/Move 등 막기
            }
        }
    }

    void EndBlockInput()
    {
        if (blockPointerDuringTransition)
        {
            overlayImage.raycastTarget = _prevRaycastTarget;
        }
        if (blockNavigationDuringTransition)
        {
            var es = eventSystem != null ? eventSystem : EventSystem.current;
            if (es != null) es.sendNavigationEvents = _prevSendNavigationEvents;
        }
    }
    // ------------------------------

    // ===== 공통: 즉시 오픈(대기 없음) 버전 =====
    public void Play(Action atDark = null, Action onFinished = null)
    {
        if (!overlayImage || !mat) return;

        // 정리
        tweenClose?.Kill();
        tweenOpen?.Kill();
        if (_waitRoutine != null) { StopCoroutine(_waitRoutine); _waitRoutine = null; }
        _cts?.Cancel(); _cts?.Dispose(); _cts = null;

        UpdateRectSize();
        overlayImage.enabled = true;

        // 입력 차단 시작
        BeginBlockInput();

        float r = startRadius;
        mat.SetFloat(_radiusId, r);

        // 밝음(startRadius) → 어두움(endRadius)
        tweenClose = DOTween.To(() => r, v =>
        {
            r = v;
            mat.SetFloat(_radiusId, r);
        }, endRadius, closeDuration)
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            // 완전 어두움
            ToggleAtDark();
            try { atDark?.Invoke(); } catch (Exception e) { Debug.LogException(e); }

            // 즉시 다시 밝아짐
            StartOpen(r, onFinished);
        });
    }

    // ===== 코루틴이 끝난 뒤 밝아지는 버전 =====
    // 예: transition.PlayWaitCoroutine(() => StageManager.Instance.StageStartRoutine());
    public void PlayWaitCoroutine(Func<IEnumerator> atDarkCoroutine, Action onFinished = null, int settleFramesBeforeOpen = 1, Action beforeOpen = null)
    {
        if (!overlayImage || !mat) return;

        tweenClose?.Kill();
        tweenOpen?.Kill();
        if (_waitRoutine != null) { StopCoroutine(_waitRoutine); _waitRoutine = null; }
        _cts?.Cancel(); _cts?.Dispose(); _cts = null;

        UpdateRectSize();
        overlayImage.enabled = true;
        BeginBlockInput();

        float r = startRadius;
        mat.SetFloat(_radiusId, r);

        tweenClose = DOTween.To(() => r, v =>
        {
            r = v;
            mat.SetFloat(_radiusId, r);
        }, endRadius, closeDuration)
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            ToggleAtDark();

            _waitRoutine = StartCoroutine(WaitThenOpen_Coroutine(atDarkCoroutine, r, onFinished, settleFramesBeforeOpen, beforeOpen));
        });
    }
    private IEnumerator WaitThenOpen_Coroutine(Func<IEnumerator> work, float currentRadius, Action onFinished, int settleFrames, Action beforeOpen)
    {
        // 1) 어두울 때 준비 코루틴 수행
        IEnumerator e = null;
        try { e = work?.Invoke(); } catch (Exception ex) { Debug.LogException(ex); }
        if (e != null) yield return StartCoroutine(e);

        // 2) 레이아웃/캔버스 정착(한 프레임 권장)
        for (int i = 0; i < Mathf.Max(0, settleFrames); i++)
        {
            Canvas.ForceUpdateCanvases();     // 즉시 레이아웃/그리기 데이터 갱신
            yield return null;                 // 다음 프레임까지 대기
        }

        // (선택) 오픈 직전 콜백
        beforeOpen?.Invoke();

        // 3) 밝히기 시작
        StartOpen(currentRadius, onFinished);
        _waitRoutine = null;
    }

    private IEnumerator WaitThenOpen_Coroutine(Func<IEnumerator> work, float currentRadius, Action onFinished)
    {
        IEnumerator e = null;
        try { e = work(); }
        catch (Exception ex) { Debug.LogException(ex); }

        if (e != null)
            yield return StartCoroutine(e);

        StartOpen(currentRadius, onFinished);
        _waitRoutine = null;
    }

    // ===== Task가 끝난 뒤 밝아지는 버전 =====
    // 예: transition.PlayWaitAsync(ct => StageManager.Instance.StageStartAsync(ct));
    public void PlayWaitAsync(Func<CancellationToken, Task> atDarkAsync, Action onFinished = null)
    {
        if (!overlayImage || !mat) return;

        tweenClose?.Kill();
        tweenOpen?.Kill();
        if (_waitRoutine != null) { StopCoroutine(_waitRoutine); _waitRoutine = null; }
        _cts?.Cancel(); _cts?.Dispose(); _cts = null;

        UpdateRectSize();
        overlayImage.enabled = true;
        BeginBlockInput();

        float r = startRadius;
        mat.SetFloat(_radiusId, r);

        tweenClose = DOTween.To(() => r, v =>
        {
            r = v;
            mat.SetFloat(_radiusId, r);
        }, endRadius, closeDuration)
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            ToggleAtDark();

            if (atDarkAsync == null)
            {
                StartOpen(r, onFinished);
                return;
            }

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            // Task 완료를 프레임 루프로 감시
            _waitRoutine = StartCoroutine(WaitTaskThenOpen_Coroutine(atDarkAsync, token, r, onFinished));
        });
    }

    private IEnumerator WaitTaskThenOpen_Coroutine(Func<CancellationToken, Task> work, CancellationToken token, float currentRadius, Action onFinished)
    {
        Task task = null;
        try { task = work(token); }
        catch (Exception ex) { Debug.LogException(ex); }

        if (task != null)
        {
            while (!task.IsCompleted) yield return null;
            if (task.IsFaulted) Debug.LogException(task.Exception);
        }

        StartOpen(currentRadius, onFinished);

        _waitRoutine = null;
        _cts?.Dispose(); _cts = null;
    }

    // ===== 공통 오픈 처리 =====
    private void StartOpen(float currentRadius, Action onFinished)
    {
        float r = currentRadius; // 현 값에서 시작
        tweenOpen = DOTween.To(() => r, v =>
        {
            r = v;
            mat.SetFloat(_radiusId, r);
        }, startRadius, openDuration)
        .SetEase(Ease.OutCubic)
        .SetUpdate(true)
        .OnComplete(() =>
        {
            overlayImage.enabled = false;
            EndBlockInput();
            onFinished?.Invoke();
        });
    }

    // --- 선택 API ---
    public void SetCenter01(Vector2 rect01)
    {
        center01 = rect01;
        if (mat) mat.SetVector(_centerId, new Vector4(center01.x, center01.y, 0, 0));
    }

    public void SetDarkToggleTargets(IEnumerable<GameObject> toEnable, IEnumerable<GameObject> toDisable)
    {
        enableOnDark = toEnable != null ? new List<GameObject>(toEnable) : new List<GameObject>();
        disableOnDark = toDisable != null ? new List<GameObject>(toDisable) : new List<GameObject>();
    }
}
