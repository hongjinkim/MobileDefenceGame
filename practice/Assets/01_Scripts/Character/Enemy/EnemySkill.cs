using System;
using System.Collections;
using UnityEditor;
public class EnemySkill : State<EnemyControl>
{
    public override void Enter(EnemyControl entity)
    {
        entity.LookAtTarget(entity.Target.transform.position); // 바라보는 방향 설정(공격 시작시에 설정)
    }

    public override void Execute(EnemyControl entity)
    {
    }

    public override void Exit(EnemyControl entity)
    {
        entity.State.SkillTermTimer = 0;
    }
}