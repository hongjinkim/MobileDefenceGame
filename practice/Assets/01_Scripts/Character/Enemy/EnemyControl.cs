using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.UIElements;

public class EnemyControl : CharacterBase
{
    public EEnemyType EnemyType;

    private State<EnemyControl>[] States;
    private State<EnemyControl> CurrentState;

    public void InitEnemy()
	{

	}

    // State 변경
    public void ChangeState(EActType NewState)
    {
        // 바꾸려는 상태가 비어있는 경우
        if (States[(int)NewState] == null)
            return;

        // 현재 재생중인 상태가 존재하면 기존 상태 종료
        if (CurrentState != null)
        {
            CurrentState.Exit(this);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        CurrentState = States[(int)NewState];
        CurrentState.Enter(this);
    }
}
