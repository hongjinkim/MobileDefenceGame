using System.Collections;
using UnityEngine;

public class HeroSkill : State<HeroControl>
{
    public override void Enter(HeroControl entity)
    {
        //entity.AttackSound.Play(); // 사운드

        if (entity.Target != null) { entity.LookAtTarget(entity.Target.CenterPoint.transform.position); } // 바라보는 방향 설정(공격 시작시에 설정)
        //entity.Rigid.velocity = Vector3.zero; //콜라이더 충돌로 계속 밀리는 현상이 있어서, 속도 리셋
    }

    public override void Execute(HeroControl entity)
    {
    }

    public override void Exit(HeroControl entity)
    {
        entity.State.SkillTermTimer = 0;
    }
}