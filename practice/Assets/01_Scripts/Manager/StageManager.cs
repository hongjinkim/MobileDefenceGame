using Unity.VisualScripting;
using UnityEngine;

public class StageManager : BasicSingleton<StageManager>
{
    [SerializeField]private ChapterData _stageData;
    public static ChapterData GetStageData()
    {
        return Instance._stageData;
    }
}
