using System.Collections;
using UnityEngine;

public class HeroInit : State<HeroControl>
{
    public override void Enter(HeroControl entity)
    {
        
    }
    public override void Execute(HeroControl entity)
    {
        if (entity.State.InitTimer >= entity.State.InitTime)
        {
            entity.ChangeState(EActType.Idle);
            entity.State.IsInitialized = true;
        }

    }

    public override void Exit(HeroControl entity)
    {
    }
}