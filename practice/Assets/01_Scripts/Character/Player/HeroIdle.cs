using System.Collections;
using UnityEngine;

public class HeroIdle : State<HeroControl>
{
    public override void Enter(HeroControl entity)
    {
        
    }

    public override void Execute(HeroControl entity)
    {
        // 무적상태면 못움직임(스폰 직후 등)
        if (entity.State.Invincible == true) return;

        // 타겟 없으면 타겟 변경
        if (entity.Target == null || entity.Target.gameObject.activeSelf == false)
        {
            // 현재 맵에 적이 없으면 null값이 들어감
            EnemyControl Enemy = entity.SearchTarget();

            if (Enemy == null) { entity.ChangeState(EActType.Idle); }
            else { entity.Target = Enemy; }
        }

        //타겟이 있는 경우
        else
        {
            float Distance = Vector3.Distance(entity.CenterPoint.position, entity.Target.CenterPoint.position);

            //사거리 안에 있으면 공격시도
            if (Distance < entity.State.AttackRange)
            {
                //스킬 가능하면 스킬발동
                if (entity.State.SkillTermTimer >= entity.State.SkillTermTime && entity.Target.State.IsLive == true && entity.State.IsHaveSkill)
                {
                    { entity.ChangeState(EActType.Skill); }
                }
                //공격 가능하면 공격발동
                if (entity.State.AttackTermTimer >= entity.State.AttackTermTime && entity.Target.State.IsLive == true)
                {
                    { entity.ChangeState(EActType.Attack); }
                }
            }
        }
    }

    public override void Exit(HeroControl entity)
    {
    }
}