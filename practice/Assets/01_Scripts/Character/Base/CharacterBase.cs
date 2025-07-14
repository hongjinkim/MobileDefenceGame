using System;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public class CharacterState
{
    //public EActType CurrentAct;
    public BigNum MaxHp;
    public BigNum CurrentHp;

    public bool IsLive = true;


    // 무적 시간
    public bool Invincible = true;
    public float InvincibleTimer = 0;
    public float InvincibleTime = 2;

    // 피격 텀
    public bool Hitable = true;
    public float HitTermTimer = 0;
    public float HitTermTime = 0.5f;

    // 공격 텀
    public float AttackTermTimer = 0;
    public float AttackTermTime = 0.5f;

    // 시작 시 시간
    public float InitTimer = 0;
    public float InitTime = 1f;
    public bool IsInitialized = false;

    // 사망 시간
    public float DieTimer = 0;
    public float DieTime = 1f;           // 사망 이펙트 대기 시간
    //public float RebirthTime = 3f;

    //스킬 사용 후 대기시간
    public bool IsHaveSkill = false;
    public float SkillTermTimer = 0f;
    public float SkillTermTime = 3f;

    public float AttackRange = 4f; //  공격 사거리
    public float SkillRange = 10f; // 스킬 사거리
    public float Speed = 10f;
    public Vector3 Scale;
    public bool NoneAttack;     // 공격 유무
    public bool NoneMove;       // Idle상태 유지

    public string CurrActName; // 현재 액션 이름
}

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] public CharacterBase Target;
    [SerializeField] protected Collider AttackCollider;
    public Transform CenterPoint;
    public Transform Anchor;

    public CharacterState State = new CharacterState();
    protected bool isEnemy;

    protected void Awake()
    {

    }

    protected void Update()
    {
        State.AttackTermTimer += Time.deltaTime;
        //State.AttackTimer += Time.deltaTime;
        State.HitTermTimer += Time.deltaTime;
        State.InvincibleTimer += Time.deltaTime;
        State.DieTimer += Time.deltaTime;
        if (!State.IsInitialized)
            State.InitTimer += Time.deltaTime;
        //State.TurnTimer += Time.deltaTime;
        //State.SkillWaitTimer += Time.deltaTime;

        SetTimer();
    }

    // 각종 타이머 계산
    private void SetTimer()
    {
        if (State.HitTermTimer >= State.HitTermTime)
            State.Hitable = true;
        else
            State.Hitable = false;

        if (State.InvincibleTimer >= State.InvincibleTime)
            State.Invincible = false;
        else
            State.Invincible = true;
    }
	public virtual void TakeHit(AttackInfo HitInfo)
	{

	}
	public virtual void Die()
    {
        
    }
    //protected virtual void KnockBack(float knockBackForce, EAttackerType attackerType, int heroIndex) { }

    // 타겟 방향 바라보기

    public void LookAtTarget(Vector3 TargetPos)
    {
        //Debug.Log($"{TargetPos}");

        // 현재 위치에서 목표 위치를 향하는 방향 벡터 계산
        Vector3 directionToTarget = (TargetPos - Anchor.transform.position).normalized;

        // 수직 회전은 제외하고 수평 방향만 고려 (y 값을 0으로)
        directionToTarget.y = 0;

        // 방향 벡터가 유효한 경우에만 회전 실행
        if (directionToTarget != Vector3.zero)
        {
            // 목표 회전 각도 계산 (라디안에서 도수로 변환)
            var _targetRotation = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

            // 부드러운 회전 적용

            // 캐릭터 회전 적용
            Anchor.transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }
    }


    // 체력 초기화
    protected virtual void InitHP(BigNum maxHp)
    { 
        State.MaxHp = maxHp;
        State.CurrentHp = maxHp;
    }
}
