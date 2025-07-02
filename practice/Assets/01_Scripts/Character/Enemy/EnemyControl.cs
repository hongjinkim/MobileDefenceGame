using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections;

public class EnemyControl : CharacterBase
{
    public EEnemyType EnemyType;

    private State<EnemyControl>[] States;
    private State<EnemyControl> CurrentState;

    public void InitEnemy()
	{

	}

    // State 변경
    public void ChangeState(EActType NewState)
    {
        // 바꾸려는 상태가 비어있는 경우
        if (States[(int)NewState] == null)
            return;

        // 현재 재생중인 상태가 존재하면 기존 상태 종료
        if (CurrentState != null)
        {
            CurrentState.Exit(this);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        CurrentState = States[(int)NewState];
        CurrentState.Enter(this);
    }

    public new void TakeHit(AttackInfo HitInfo)
    {
        // 무적 또는 피격가능 또는 생존 체크
        if (State.Invincible == true || State.Hitable == false || State.IsLive == false)
            return;

        base.TakeHit(HitInfo);

        if (HitInfo.HitCount == 1)
        {
            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(this.transform.position + new Vector3(0, 1f, 0), HitInfo);
            FXPoolManager.Instance.Pop(HitInfo.EffectType, new Vector3(this.transform.position.x, this.transform.position.y + 2f, 0));

            if (State.CurrentHp <= 0) { Die(); } // 사망 처리
        }
        else
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(MultiHit(HitInfo));
            }
        }
    }
    // 타수 적용
    private IEnumerator MultiHit(AttackInfo HitInfo)
    {
        for (int i = 0; i < HitInfo.HitCount; i++)
        {
            if (State.IsLive == false) { yield break; } //타격중에 다른타격으로 사망시 중단

            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(this.transform.position + new Vector3(0, 0.5f + (i * 0.5f), 0), HitInfo);
            FXPoolManager.Instance.Pop(HitInfo.EffectType, new Vector3(this.transform.position.x, this.transform.position.y + 2f, 0));

            if (State.CurrentHp <= 0) { Die(); yield break; } // 사망 처리

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void HandleEvent(string eventName)
    {

    }

    // 모션 끝
    protected override void Finish(EActType ActType)
    {

    }
}
