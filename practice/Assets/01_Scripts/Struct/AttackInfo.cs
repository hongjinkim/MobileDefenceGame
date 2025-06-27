using UnityEngine;

public struct AttackInfo
{
    public BigNum Damage;
    public float KnockbackForce;
    public bool IsMiss;
    public EAttackType AttackType;
    public EFXPoolType EffectType;
    public int HitCount;
    public EAttackerType AttackerType;
    public Vector3 AttackerWorldPosition;
}

public enum EAttackerType
{
    None,
    Hero,
    Enemy,
    Boss
}
