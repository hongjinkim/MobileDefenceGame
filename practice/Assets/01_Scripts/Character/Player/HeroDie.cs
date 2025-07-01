using Kitty;
using System.Collections;
using UnityEngine;

public class HeroDie : State<HeroControl>
{
    private bool DieEffect = false;

    public override void Enter(HeroControl entity)
    {
        entity.DieFX();

        entity.State.DieTimer = 0;
        DieEffect = false;
    }

    public override void Execute(HeroControl entity)
    {
        if (entity.State.DieTimer >= entity.State.DieTime && DieEffect == false)
        {
            DieEffect = true;
        }
    }

    public override void Exit(HeroControl entity)
    {
    }
}