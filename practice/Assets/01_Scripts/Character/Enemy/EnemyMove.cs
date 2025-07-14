using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMove : State<EnemyControl>
{
    public override void Enter(EnemyControl entity)
    {

    }

    public override void Execute(EnemyControl entity)
    {
        if (entity.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

        // 타겟 없으면 idle전환
        if (entity.Target == null) { entity.ChangeState(EActType.Idle); }

        // 타켓이 있는 경우
        else
        {
            entity.SetFace(entity.faces.WalkFace); // 걷는 표정으로 변경

            entity.agent.isStopped = false;
            entity.agent.updateRotation = true;

            entity.animator.SetFloat("Speed", entity.State.Speed);

            float Distance = Vector3.Distance(entity.CenterPoint.position, entity.Target.CenterPoint.position);

            //사거리 안에 있는 경우
            if (Distance < entity.State.AttackRange)
            {
                //공격텀인 경우 공격 발동
                if (entity.State.IsHaveSkill && entity.State.SkillTermTimer >= entity.State.SkillTermTime)
                {
                    //스킬 사용
                    entity.ChangeState(EActType.Skill);
                }
                else if (entity.State.AttackTermTimer >= entity.State.AttackTermTime) { entity.ChangeState(EActType.Attack); }

                //공격텀이 아닌 경우 idle전환
                else { entity.ChangeState(EActType.Idle); }
            }

            //사거리밖인경우
            else
            {
                entity.LookAtTarget(entity.Target.CenterPoint.transform.position);       // 바라보는 방향 설정

                // 3D 환경에 맞게 y좌표는 고정, x와 z만 조정
                Vector3 moveDirect = entity.Target.transform.position - entity.transform.position;
                float x_pos = Mathf.Clamp(entity.transform.position.x + moveDirect.x, PositionInfo.Instance.MinPos.position.x, PositionInfo.Instance.MaxPos.position.x);
                float z_pos = Mathf.Clamp(entity.transform.position.z + moveDirect.z, PositionInfo.Instance.MinPos.position.z, PositionInfo.Instance.MaxPos.position.z);
                float y_pos = entity.transform.position.y; // y좌표 고정

                Vector3 clampPos = new Vector3(x_pos, y_pos, z_pos);
                entity.transform.position = Vector3.MoveTowards(entity.transform.position, clampPos, Time.deltaTime * entity.State.Speed);
            } 
        }
    }

    public override void Exit(EnemyControl entity)
    {
    }
}
