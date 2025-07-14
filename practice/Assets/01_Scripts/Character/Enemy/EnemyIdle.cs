using System.Collections;
using UnityEngine;

public class EnemyIdle : State<EnemyControl>
{
    public override void Enter(EnemyControl entity)
    {

    }

    public override void Execute(EnemyControl entity)
    {
        if (entity.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
           
        if (entity.State.Invincible == true) return;

        if (entity.State.NoneMove == true) return;

        entity.StopAgent();
        entity.SetFace(entity.faces.Idleface);

        //타켓이 없는 경우 타겟 서치
        if (entity.Target == null) { entity.Target = entity.NearPlayer(); }

        //타겟이 있는 경우
        else
        {
            float Distance = Vector3.Distance(entity.CenterPoint.position, entity.Target.CenterPoint.position);

            //사거리 안에 있는 경우
            if (Distance < entity.State.AttackRange)
            {
                if(entity.State.IsHaveSkill && entity.State.SkillTermTimer >= entity.State.SkillTermTime)
                {
                    //스킬 사용
                    entity.ChangeState(EActType.Skill);
                }
                //공격 가능하면 공격발동
                else if (entity.State.AttackTermTimer >= entity.State.AttackTermTime)
                {
                    entity.ChangeState(EActType.Attack); 
                }
            }
            //거리가 멀면 이동
            else { entity.ChangeState(EActType.Move); }
        }
    }

    public override void Exit(EnemyControl entity)
    {
    }
}
