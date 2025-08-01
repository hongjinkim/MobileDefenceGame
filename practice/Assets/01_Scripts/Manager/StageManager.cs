
using Sirenix.OdinInspector;
using UnityEngine;

public class StageManager : BasicSingleton<StageManager>
{

    [ReadOnly] private int currentStage;

    [ShowInInspector]
    private StageValue stageValue;

    public void StageStart()
    {
        currentStage = PlayerManager.Instance.currentStage;
        Debug.Log($"stage {currentStage} started");
        if (DataBase.TryGetStageValue(currentStage, out var value))
        {
            stageValue = value;
        }
        else
        {
            Debug.Log($"id : {currentStage}에 해당하는 데이터를 불러오는데 실패");
        }
    }
}
