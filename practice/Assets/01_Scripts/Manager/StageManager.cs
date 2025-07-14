
using UnityEngine;

public class StageManager : BasicSingleton<StageManager>
{
    [SerializeField]private ChapterData _stageData;
    public static ChapterData GetStageData()
    {
        return Instance._stageData;
    }

    public static void StageStart()
    {
        // Initialize the stage data or perform any setup required for the stage
        Debug.Log("Stage started with data: " + GetStageData());
        

    }
}
