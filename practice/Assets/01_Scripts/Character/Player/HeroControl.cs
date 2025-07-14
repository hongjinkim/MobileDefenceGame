using System;
using UnityEngine;
using UnityEngine.AI;

public class HeroControl : CharacterBase
{
    [Header("�̺�Ʈ")]
    public VoidEventChannelSO HeroDieEvent;

    [Header("UI")]
    [SerializeField] protected RectTransform HP_HUD;
    [SerializeField] protected RectTransform HP_HUD_After;
    [SerializeField] protected RoundedFillUI HP_HUD_Fill;
    [SerializeField] protected RoundedFillUI HP_HUD_Fill_After;

    [Header("����")]
    [SerializeField] private AudioPlayerSingle DieSound;
    [SerializeField] private AudioPlayerSingle HitSound;
    [SerializeField] private AudioPlayerSingle AppearSound;

    [Header("ĳ���� ����")]
    [SerializeField] private CapsuleCollider BodyCollider;

    private State<HeroControl>[] States;
    private State<HeroControl> CurrentState;

    public int HeroIndex;

    private PlayerData Player => DataBase.GetPlayerData();
    private InitialData Initial => DataBase.GetInitialData();

    [Header("�ִϸ��̼� ����")]
    public Animator animator;
    private Vector3 originPos;
    [HideInInspector]public int AttackType = 0; // ���� Ÿ��


    private void OnEnable()
    {
        AddListener();
    }

    private void OnDisable()
    {
        RemoveListener();
    }

    private void AddListener()
    {
        EnemyManager.Instance.EnemyGenEvent.AddListener(RefreshTarget);
    }

    private void RemoveListener()
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
        State.InvincibleTimer = 0;     // ���� �����ð� Ÿ�̸�
        State.IsLive = true;
        Target = null;

        // �ʱ���� ����
        InitHP(100); // ü�� ����
        AttackCollider.gameObject.SetActive(false);
        ChangeState(EActType.Init);
    }

    protected override void InitHP(BigNum maxHp)
    {
        HP_HUD.gameObject.SetActive(true);
        HP_HUD_After.gameObject.SetActive(true);

        base.InitHP(maxHp);

        UpdateHpBar();
    }

    // ü�¹� ����
    private void UpdateHpBar()
    {
        if (isEnemy)
            return;
        if (State.CurrentHp >= State.MaxHp)
        {
            HP_HUD_Fill.SetProgress(1f);
            HP_HUD_Fill_After.SetProgress(1f);
            HP_HUD_Fill_After.StopAfterSlide();
        }
        else if (State.CurrentHp <= 0 || State.CurrentHp * 1000000000 < State.MaxHp)
        {
            HP_HUD_Fill.SetProgress(0f);
            HP_HUD_Fill_After.SetProgress_After(0f);
        }
        else
        {
            float ratio = (float)(double)(State.CurrentHp / State.MaxHp);
            HP_HUD_Fill.SetProgress(ratio);
            HP_HUD_Fill_After.SetProgress_After(ratio);
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

    protected new void Update()
    {
        base.Update();
        if (CurrentState != null) CurrentState.Execute(this);
    }

    // ĳ���� �ǰ�
    public override void TakeHit(AttackInfo HitInfo)
    {
        //Debug.Log($"{State.Invincible} / {State.Hitable} / {State.IsLive}");
        // ���� �Ǵ� �ǰݰ��� �Ǵ� ���� üũ
        if (State.Invincible == true || State.Hitable == false || State.IsLive == false)
            return;

        base.TakeHit(HitInfo);

        State.CurrentHp -= HitInfo.Damage;

        FXPoolManager.Instance.PopDamageText(CenterPoint.transform.position.ProjectTo2D(), HitInf o);
        //FXPoolManager.Instance.Pop(HitInfo.EffectType, CenterPoint.transform.position);

        UpdateHpBar();

        // ��� ó��
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



    //���ȿ��
    public void DieFX()
    {
        //FXPoolManager.Instance.Pop(EFXPoolType.UnitDieEffect, this.transform.position + new Vector3(0, -1, 0));
    }

        
    public void RefreshTarget(Void _)
    {
        Target = SearchTarget();
    }

    public EnemyControl SearchTarget() => EnemyManager.Instance.FindNearTarget(this.CenterPoint.position, HeroIndex);

    // ����ΰ� Ȱ�� ������ �����ΰ�
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
        State.AttackRange = Initial.AttackRange;
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
}
