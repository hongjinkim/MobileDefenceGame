using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
public class EnemyAttack : State<EnemyControl>
{
    public override void Enter(EnemyControl entity)
    {
        entity.LookAtTarget(entity.Target.CenterPoint.position); // 바라보는 방향 설정(공격 시작시에 설정)

    }

    public override void Execute(EnemyControl entity)
    {
        if (entity.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        entity.StopAgent();
        entity.SetFace(entity.faces.attackFace);
        entity.animator.SetTrigger("Attack");
    }

    public override void Exit(EnemyControl entity)
    {
        entity.State.AttackTermTimer = 0;
    }
}