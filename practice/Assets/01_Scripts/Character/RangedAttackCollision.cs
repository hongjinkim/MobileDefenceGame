using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackCollision : AttackCollision
{
    [SerializeField] bool IsPushObject = false;       // ¼Ò¸ê ¿©ºÎ

    protected override void GiveDamage(Collider collision)
    {
        base.GiveDamage(collision);
        // ¼Ò¸ê
        if (IsPushObject == true)
            PushObject();
    }
}
