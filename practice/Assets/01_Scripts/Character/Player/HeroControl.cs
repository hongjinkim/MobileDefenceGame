using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : CharacterBase
{
    [SerializeField] private AudioPlayerSingle DieSound;
    [SerializeField] private AudioPlayerSingle HitSound;
    [SerializeField] private AudioPlayerSingle AppearSound;
    [SerializeField] private CapsuleCollider BodyCollider;

    private State<HeroControl>[] States;
    private State<HeroControl> CurrentState;

    private bool isDashing;

    public int HeroIndex;

    private PlayerData Player => DataManager.GetPlayerData();
    private InitialData Initial => DataManager.GetInitialData();


    private void OnEnable()
    {
            
    }

    private void OnDisable()
    {
            
    }

    protected new void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Start()
    {
        Init();
    }


    private void Setup()
    {
        States = new State<HeroControl>[5];
        States[(int)EActType.Init] = new HeroInit();
        States[(int)EActType.Idle] = new HeroIdle();
        States[(int)EActType.Attack] = new HeroAttack();
        States[(int)EActType.Skill] = new HeroSkill();
        States[(int)EActType.Die] = new HeroDie();

        isEnemy = false;
    }

    public void Init()
    {
        State.HitTermTime = 0.5f;
        State.HitTermTimer = 0;
        State.InvincibleTime = 2;
        State.InvincibleTimer = 0;     // 스폰 무적시간 타이머
        Rigid.velocity = Vector3.zero; // 완전히 멈추도록 설정
        State.IsLive = true;
        Target = null;

        // 초기상태 설정
        InitHP(100f); // 체력 설정
        AttackCollider.gameObject.SetActive(false);
        ChangeState(EActType.Init);
    }


    // 상태 변경
    public void ChangeState(EActType NewState)
    {
        // 바꾸려는 상태가 비어있는 경우
        if (States[(int)NewState] == null) return;

        // 현재 재생중인 상태가 존재하면 기존 상태 종료
        if (CurrentState != null) { CurrentState.Exit(this); }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        CurrentState = States[(int)NewState];
        CurrentState.Enter(this);
    }

    protected new void Update()
    {
        base.Update();
        if (CurrentState != null) CurrentState.Execute(this);
    }

    // 캐릭터 피격
    public override void TakeHit(AttackInfo HitInfo)
    {
        //Debug.Log($"{State.Invincible} / {State.Hitable} / {State.IsLive}");
        // 무적 또는 피격가능 또는 생존 체크
        if (State.Invincible == true || State.Hitable == false || State.IsLive == false)
            return;

        base.TakeHit(HitInfo);

        State.CurrentHp -= HitInfo.Damage;

        FXPoolManager.Instance.PopDamageText(this.transform.position + new Vector3(0, 2f, 0), HitInfo);
        FXPoolManager.Instance.Pop(HitInfo.EffectType, this.transform.position + new Vector3(0, 1f, 0));

        UpdateHp();

        // 사망 처리
        if (State.CurrentHp <= 0)
            Die();
    }


    public override void Die()
    {
        base.Die();

        ChangeState(EActType.Die);
        AttackCollider.gameObject.SetActive(false);
        State.IsLive = false;
    }



    //사망효과
    public void DieFX()
    {
        //FXPoolManager.Instance.Pop(EFXPoolType.UnitDieEffect, this.transform.position + new Vector3(0, -1, 0));
    }

        
    private void RefreshTarget()
    {
        Target = SearchTarget();
    }

    public EnemyControl SearchTarget() => EnemyManager.Instance.FindNearTarget(this.CenterPoint.position, HeroIndex);

    // 히어로가 활동 가능한 상태인가
    public bool IsHeroActive()
    {
        if (State.Invincible == true || State.IsLive == false || !gameObject.activeSelf) return false;

        else return true;
    }
   
    private void SettingAttackSpeed()
    {
        float statSpeed = StatManager.Instance.FinalAttackSpeed;
        State.AttackTermTime = Initial.AttackSpeed / statSpeed;
    }

    private void SettingRange()
    {
        State.Range = Initial.AttackRange;
    } 
}
