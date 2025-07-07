using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HeroControl : CharacterBase
{
    [SerializeField] private AudioPlayerSingle DieSound;
    [SerializeField] private AudioPlayerSingle HitSound;
    [SerializeField] private AudioPlayerSingle AppearSound;
    [SerializeField] private CapsuleCollider BodyCollider;

    private State<HeroControl>[] States;
    private State<HeroControl> CurrentState;

    public int HeroIndex;

    private PlayerData Player => DataBase.GetPlayerData();
    private InitialData Initial => DataBase.GetInitialData();

    [Header("애니메이션 설정")]
    public Animator animator;
    private Vector3 originPos;
    [HideInInspector]public int AttackType = 0; // 공격 타입


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
        State.CurrActName = NewState.DisplayName(); // 현재 액션 이름 업데이트
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

    // Animation Event
    public void AlertObservers(string message)
    {
        if (message.Equals("AnimationAttackStarted"))
        {
            if (Target != null)
            {
                AttackCollider.transform.localPosition = CenterPoint.localPosition + Vector3.forward / 2;
            }
            AttackCollider.gameObject.SetActive(true);
        }

        if (message.Equals("AnimationDamageEnded"))
        {
            float distanceOrg = Vector3.Distance(transform.position, originPos);
            if (distanceOrg > 1f)
            {
                ChangeState(EActType.Move);
            }
            else ChangeState(EActType.Idle);

            //Debug.Log("DamageAnimationEnded");
        }

        if (message.Equals("AnimationAttackEnded"))
        {
            AttackCollider.gameObject.SetActive(false);
            ChangeState(EActType.Idle);
        }
    }
}
