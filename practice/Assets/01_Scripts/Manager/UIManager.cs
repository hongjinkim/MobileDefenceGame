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
        // 어두워지는 순간 패널 전환(선택) ? 필요 없으면 제거
        transition.SetDarkToggleTargets(
            toEnable: new[] { stagePanel },
            toDisable: new[] { lobbyPanel }
        );

        // 트랜지션은 범용 Play, 여기서만 끝난 뒤 StageStart 수행
        transition.Play(() =>
        {
            StageManager.Instance.StageStart();
        });
    }

    // 범용: 트랜지션만 재생하고 아무 것도 안 하고 싶을 때
    public void PlayOnlyTransition()
    {
        transition.SetDarkToggleTargets(null, null); // 토글 없이
        transition.Play(); // 콜백 없음
    }
}
