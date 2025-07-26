using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemySpawnData
{
    public string EnemyID;// ID of the enemy to spawn
    public int SpawnCount; // Number of enemies to spawn
    public float SpawnDelay; // Delay before spawning the next enemy
}
[Serializable]
public class StageWaveValue
{
    public ESpawnPattern SpawnPattern; // Pattern of enemy spawning (e.g., random, sequential)
    public List<EnemySpawnData> SpawnDatas = new List<EnemySpawnData>();
}