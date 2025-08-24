using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BasicSingleton<UIManager>
{
    [Header("UI Panel")]
    [SerializeField] private GameObject lobbyPanel; 
    [SerializeField] private GameObject stagePanel;

    [Header("FX")]
    [SerializeField] private CircleTransition transition;

    public void StartStageTransition()
    {
        transition.SetDarkToggleTargets(
            toEnable: new[] { stagePanel },
            toDisable: new[] { lobbyPanel }
        );

        // 어두워진 뒤 준비 + UI 프라임(그 프레임에 바로 보이도록) → 1프레임 정착 → 밝게
        transition.PlayWaitCoroutine(
        atDarkCoroutine: () => StageManager.Instance.StagePrepareRoutine(),
        onFinished: () => StageManager.Instance.StageStart()
        );
    }

    // 범용: 트랜지션만 재생하고 아무 것도 안 하고 싶을 때
    public void PlayOnlyTransition()
    {
        transition.SetDarkToggleTargets(null, null); // 토글 없이
        transition.Play(); // 콜백 없음
    }
}
