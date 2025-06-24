using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> where T : CharacterBase
{
    // 해당 상태를 시작할 때 1회 호출
    public virtual void Enter(T entity)
    {
    }

    // 해당 상태를 업데이트할 때 매 프레임 호출
    public virtual void Execute(T entity)
    {
    }

    // 해당 상태를 업데이트할 때 물리계산 시 호출
    public virtual void FixedExecute(T entity)
    {
    }

    // 해당 상태를 종료할 때 1회 호출
    public virtual void Exit(T entity)
    {
    }
}
