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
    public BigNum DropGold = 10;
    public BigNum DropHeroExp = 10;
    public BigNum DropExp = 10;
    public int Stage;
}

public class EnemyControl : CharacterBase
{

    [Header("����")]
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

    [Header("�ִϸ��̼� ����")]
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
        State.HitTermTime = 0.5f;
        State.HitTermTimer = 0;
        State.InvincibleTime = 2;
        State.InvincibleTimer = 0;     // ���� �����ð� Ÿ�̸�
        State.IsLive = true;
        Target = null;

        // �ʱ���� ����
        InitHP(Info.MaxHp); // ü�� ����
        AttackCollider.gameObject.SetActive(false);
        ChangeState(EActType.Init);
    }


    //����� �÷��̾� Ž��
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
        // ���� �Ǵ� �ǰݰ��� �Ǵ� ���� üũ
        if (State.Invincible == true || State.Hitable == false || State.IsLive == false)
            return;

        base.TakeHit(HitInfo);

        //HitSound.Play();
        // �ǰ� �ִϸ��̼��� ������� ��� �ߺ� ���� ����
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

            if (State.CurrentHp <= 0) { Die(); } // ��� ó��
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

    // Ÿ�� ����
    private IEnumerator MultiHit(AttackInfo HitInfo)
    {
        for (int i = 0; i < HitInfo.HitCount; i++)
        {
            if (State.IsLive == false) { yield break; } //Ÿ���߿� �ٸ�Ÿ������ ����� �ߴ�

            State.CurrentHp -= HitInfo.Damage;
            FXPoolManager.Instance.PopDamageText(CenterPoint.transform.position.ProjectTo2D(), HitInfo);
            FXPoolManager.Instance.Pop(HitInfo.EffectType, this.transform.position);

            if (State.CurrentHp <= 0) { Die(); yield break; } // ��� ó��

            yield return new WaitForSeconds(0.1f);
        }
    }

    // ���� ����
    public void ChangeState(EActType NewState)
    {
        // �ٲٷ��� ���°� ����ִ� ���
        if (States[(int)NewState] == null)
            return;

        // ���� ������� ���°� �����ϸ� ���� ���� ����
        if (CurrentState != null)
        {
            CurrentState.Exit(this);
        }

        // ���ο� ���·� �����ϰ�, ���� �ٲ� ������ Enter() �޼ҵ� ȣ��
        CurrentState = States[(int)NewState];
        CurrentState.Enter(this);
        State.CurrActName = NewState.ToString(); // ���� �׼� �̸� ������Ʈ
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

        // ����(������)
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
        // ����ִ� ���� ó��(�Ϲݰ���/��Ƽ���� �ߺ� Die üũ ����)
        if (State.IsLive == false) return;

        // �ǰ� ����Ʈ
        //FXPoolManager.Instance.Pop(EFXPoolType.DestroyEnemy, new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0));


        //DieSound.Play();
        ChangeState(EActType.Die);
        MonsterCollider.enabled = false;
        State.IsLive = false;
        EnemyManager.Instance.EnemyDeath(this);

        // 1. ��尪�� ���⼭ �����ϵ���(����������)
        // 2. ����� ����(�������ʹ� �����������Ϳ� ������� �ٸ�)
    }

    // ��� �ִϸ��̼��� ���� �ð� �ʿ�
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
                AttackCollider.GetComponent<CapsuleCollider>().radius = State.Range;
                AttackCollider.transform.localPosition = CenterPoint.localPosition + Vector3.forward / 4;
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
