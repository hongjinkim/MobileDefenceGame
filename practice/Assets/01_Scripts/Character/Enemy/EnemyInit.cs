using System.Collections;
using UnityEngine;

public class EnemyInit : State<EnemyControl>
{
    public override void Enter(EnemyControl entity)
    {
        entity.State.InitTimer = 0;
    }

    public override void Execute(EnemyControl entity)
    {
        if (entity.State.InitTimer >= entity.State.InitTime)
        {
            entity.ChangeState(EActType.Idle);
            entity.State.IsInitialized = true;
        }
    }

    public override void Exit(EnemyControl entity)
    {
    }
}
