using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.VersionControl.Asset;

public class EnemyInfo
{ 
    public EEnemyType EnemyType;
    public BigNum AttackPower = 2;
    public BigNum MaxHp = 100;
    public BigNum DropGold = 0;
    public int Stage;
}

public class EnemyControl : CharacterBase
{

    [Header("사운드")]
    [SerializeField] private AudioPlayerSingle DieSound;
    [SerializeField] private AudioPlayerSingle HitSound;
    [SerializeField] public AudioPlayerSingle AttackSound_Boss;
    [SerializeField] public AudioPlayerSingle AttackSound_Monster;


    public EnemyInfo Info = new EnemyInfo();

    private State<EnemyControl>[] States;
    private State<EnemyControl> CurrentState;
    private CapsuleCollider MonsterCollider;

    private PlayerData Player => DataBase.GetPlayerData();
    private InitialData Inital => DataBase.GetInitialData();

    [Header("애니메이션 설정")]
    public Face faces;
    public GameObject SmileBody;
    public Animator animator;
    public NavMeshAgent agent;
    private Material faceMaterial;
    private Vector3 originPos;

    void Start()
    {
        originPos = transform.position;
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
    }

    public void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

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
        State.IsInitialized = false;
        State.InitTimer = 0;
        ChangeState(EActType.Init);
        
        InitHP(Info.MaxHp);
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

        //HitSound.Play();
        // 피격 애니메이션이 재생중인 경우 중복 실행 방지
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                    || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;

        StopAgent();
        animator.SetTrigger("Damage");
        //animator.SetInteger("DamageType", damType);
        SetFace(faces.damageFace);

        if (HitInfo.HitCount == 1)
        {
            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(CenterPoint.transform.position.ProjectTo2D(), HitInfo);
            //FXPoolManager.Instance.Pop(HitInfo.EffectType, CenterPoint.transform.position);

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
    public void StopAgent()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
        agent.updateRotation = false;
    }

    // 타수 적용
    private IEnumerator MultiHit(AttackInfo HitInfo)
    {
        for (int i = 0; i < HitInfo.HitCount; i++)
        {
            if (State.IsLive == false) { yield break; } //타격중에 다른타격으로 사망시 중단

            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(this.transform.position, HitInfo);
            FXPoolManager.Instance.Pop(HitInfo.EffectType, this.transform.position);

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
        State.CurrActName = NewState.ToString(); // 현재 액션 이름 업데이트
    }

    public void StopAttackWhenMainHeroDie()
    {
        Target = null;
        ChangeState(EActType.Idle);
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


        //DieSound.Play();
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

    // Animation Event
    public void AlertObservers(string message)
    {
        if (message.Equals("AnimationAttackStarted"))
        {
            if (Target != null)
            {
                AttackCollider.transform.localPosition = CenterPoint.localPosition + Vector3.forward/2;
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
    void OnAnimatorMove()
    {
        // apply root motion to AI
        //Vector3 position = animator.rootPosition;
        //position.y = agent.nextPosition.y;
        //transform.position = position;
        //agent.nextPosition = transform.position;
    }

}
