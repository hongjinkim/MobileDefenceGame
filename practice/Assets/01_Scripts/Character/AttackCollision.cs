using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AttackCollision : MonoBehaviour
{
    [SerializeField] CharacterBase Attacker;    // 공격하는 개체
    [SerializeField] private TargetType TargetTag;
    private enum TargetType { Enemy, Hero };
    public int maxTartetCount = 1; // 최대 타겟 수
    private int currentTargetCount; // 현재 타겟 수

    private void OnEnable()
    {
        currentTargetCount = 0;
    }


    private void OnTriggerEnter(Collider collision)
    {
        GiveDamage(collision);
    }

    protected virtual void GiveDamage(Collider collision)
    {
        AttackInfo AttackInfo = new AttackInfo();
        AttackInfo.AttackerWorldPosition = Attacker.transform.position;

        if (collision.CompareTag(TargetTag.ToString()) == false)
            return;
        if (currentTargetCount >= maxTartetCount)
            return;

        currentTargetCount++;

        switch (TargetTag)
        {
            // 플레이어 -> 적
            case TargetType.Enemy:

                // 기본 공격
                //AttackInfo.Damage = StatManager.Instance.FinalAttack_Melee;
                AttackInfo.Damage = Random.Range(1, 10);    // 스탯 연결 전 임시 공격력
                AttackInfo.AttackType = EAttackType.Normal;
                AttackInfo.AttackerType = EAttackerType.Hero;

                // 치명타 체크
                //float CriRandom = Random.Range(0, 100f);
                //float PlayerCri = StatManager.Instance.FinalCriticalRate;
                //if (CriRandom < PlayerCri)
                //{
                //    AttackInfo.Damage = AttackInfo.Damage * StatManager.Instance.FinalCritical;
                //    AttackInfo.AttackType = EAttackType.Critical;
                //}

                //AttackInfo.EffectType = EFXPoolType.HitEffect_Orange;
                AttackInfo.HitCount = 1;

                EnemyControl Target = collision.gameObject.GetComponent<EnemyControl>();

                Target.TakeHit(AttackInfo);

                break;
            // 적 -> 플레이어
            case TargetType.Hero:
                EnemyControl Enemy = Attacker.GetComponent<EnemyControl>();

                if (!Enemy.Target) { return; } //딜레이 사이에 죽어서 target이 제거된경우 return
                if (Enemy.Target != collision.GetComponent<CharacterBase>()) { return; } //타겟만 때리기

                AttackInfo.Damage = Enemy.Info.AttackPower;
                AttackInfo.AttackType = EAttackType.Normal;
                //AttackInfo.EffectType = EFXPoolType.HitEffect_White;
                AttackInfo.HitCount = 1;

                Enemy.Target.TakeHit(AttackInfo);

                break;
            default:
                break;

            
        }
    }

    // 공격 오브젝트 소멸
    protected virtual void PushObject()
    {

    }
}
