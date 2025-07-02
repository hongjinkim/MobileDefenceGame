using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawnEnemy : UIButton
{

    public ESpawnPattern spawnPattern = ESpawnPattern.Random;
    public float patternRadius;
    public int enemyCount;

#if DEBUG_ON
    protected override void OnClicked()
    {
        EnemyManager.Instance.SpawnEnemy(spawnPattern, patternRadius, enemyCount);
    }
#endif

}
