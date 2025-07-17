using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemySpawnData
{
    public string EnemyID { get; set; } // ID of the enemy to spawn
    public int SpawnCount { get; set; } // Number of enemies to spawn
    public float SpawnDelay { get; set; } // Delay before spawning the next enemy
}
[Serializable]
public class StageWaveValue
{
    public int ID { get; set; } // Wave ID 
    public List<EnemySpawnData> spawnDatas = new List<EnemySpawnData>();
}
