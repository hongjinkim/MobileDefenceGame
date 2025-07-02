using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.VersionControl.Asset;

public class EnemyInfo
{
    public EEnemyType EnemyType;
    public BigNum AttackPower = 0;
    public BigNum MaxHp = 1;
    public BigNum DropGold = 0;
    public int Stage;
}

public class EnemyControl : CharacterBase
{
    [SerializeField] private AudioPlayerSingle DieSound;
    [SerializeField] private AudioPlayerSingle HitSound;
    [SerializeField] public AudioPlayerSingle AttackSound_Boss;
    [SerializeField] public AudioPlayerSingle AttackSound_Monster;


    public EnemyInfo Info = new EnemyInfo();
    public Vector3 StayTarget = Vector3.zero;

    private State<EnemyControl>[] States;
    private State<EnemyControl> CurrentState;
    private CapsuleCollider MonsterCollider;

    private PlayerData Player => DataManager.GetPlayerData();
    private InitialData Inital => DataManager.GetInitialData();

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected new void Awake()
    {
        base.Awake();
        MonsterCollider = this.GetComponent<CapsuleCollider>();
        Setup();
    }
    private void Setup()
    {
        States = new State<EnemyControl>[6];

        States[(int)EActType.Init] = new EnemyInit();
        States[(int)EActType.Idle] = new EnemyIdle();
        States[(int)EActType.Attack] = new EnemyAttack();
        States[(int)EActType.Skill] = new EnemySkill();
        States[(int)EActType.Die] = new EnemyDie();
        States[(int)EActType.Move] = new EnemyMove();

        isEnemy = true;
    }

    public void Init()
    {

    }


    //가까운 플레이어 탐색
    public CharacterBase NearPlayer()
    {
        if (InGameHeroManager.IsHeroActive())
        {
            return InGameHeroManager.FindNearTarget(this.CenterPoint.position);
        }
        else
            return null;
    }

    public new void TakeHit(AttackInfo HitInfo)
    {
        // 무적 또는 피격가능 또는 생존 체크
        if (State.Invincible == true || State.Hitable == false || State.IsLive == false)
            return;

        base.TakeHit(HitInfo);

        HitSound.Play();

        if (HitInfo.HitCount == 1)
        {
            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(this.transform.position + new Vector3(0, 1f, 0), HitInfo);
            FXPoolManager.Instance.Pop(HitInfo.EffectType, new Vector3(this.transform.position.x, this.transform.position.y + 2f, 0));

            UpdateHp();

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

            UpdateHp();

            if (State.CurrentHp <= 0) { Die(); yield break; } // 사망 처리

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 상태 변경
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

    public void StopAttackWhenMainHeroDie()
    {
        Target = null;
        ChangeState(EActType.Idle);
    }

    protected override void HandleEvent(string eventName)
    {
    }

    protected override void Finish(EActType ActType)
    {

    }

    private void LateUpdate()
    {
        base.Update();

        if (CurrentState != null)
        {
            CurrentState.Execute(this);
        }

        // 정지(스포너)
        if (State.NoneMove == true)
        {
            return;
            /*
            if (CurrentState == States[(int)EActType.Move] || CurrentState == States[(int)EActType.Stay] || CurrentState == States[(int)EActType.Attack])
                ChangeState(EActType.Idle);

            return;
            */
        }
    }


    public override void Die()
    {
        base.Die();
        // 살아있는 적만 처리(일반공격/멀티공격 중복 Die 체크 방지)
        if (State.IsLive == false) return;

        // 피격 이펙트
        //FXPoolManager.Instance.Pop(EFXPoolType.DestroyEnemy, new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0));


        DieSound.Play();
        ChangeState(EActType.Die);
        MonsterCollider.enabled = false;
        State.IsLive = false;
        EnemyManager.Instance.EnemyDeath(this);

        // 1. 골드값을 여기서 연동하도록(배율때문에)
        // 2. 드랍템 조정(던전몬스터는 스테이지몬스터와 드랍템이 다름)
    }

    // 사망 애니메이션을 위한 시간 필요
    public void DieDisable()
    {
        EnemyPoolManager.Instance.Push(gameObject, EPoolType.Enemy);
    }
 private void RangeAttack()
    {
        if (Target != null)
        {
            //var MonsterRangeAttack = FXPoolManager.Instance.Pop(EFXPoolType.MonsterRangeAttack, CenterPoint.position);
            //MonsterRangeAttack.GetComponent<MonsterRangeAttackObject>().Init(this, Target);
        }
    }
}
