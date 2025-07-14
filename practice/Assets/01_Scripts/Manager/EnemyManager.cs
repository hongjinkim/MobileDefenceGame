using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using Random = UnityEngine.Random;

// 몬스터 및 보스의 관리
public class EnemyManager : BasicSingleton<EnemyManager>
{
	[Header("이벤트")]
	public VoidEventChannelSO EnemyGenEvent;

	

	private List<EnemyControl> MonsterList = new List<EnemyControl>();
	private PlayerData Player => DataBase.GetPlayerData();
	private ChapterData Chapter => StageManager.GetStageData();
	//private WaveData[] waves;
	//private int _currentWaveIndex = 0;


	// N 스테이지 시작
	private void StartStage()
	{
		InitStage();
		StopAllCoroutines();
		//StartCoroutine(SpawnAllWaves());
	}

	private void InitStage()
	{
		ClearAllEnemy();
		//waves = Stage.GetWaveData();
	}

	private void ClearAllEnemy()
	{
		for (int i = 0; i < MonsterList.Count; i++)
		{
			EnemyPoolManager.Instance.Push(MonsterList[i].gameObject, EPoolType.Enemy);
		}
		MonsterList.Clear();
	}

	public EnemyControl FindNearTarget(Vector3 pos, int HeroIndex)
	{
		var monster = MonsterList;

		if (monster.Count <= 0)
		{
			return null;
		}

		else
		{
			EnemyControl select = monster[0];
			var distance = GetCustomDistance(pos, monster[0].CenterPoint.position);  // 1번째로 가까운 녀석
			for (int i = 0; i < monster.Count; i++)
			{
				var monsterDistance = GetCustomDistance(pos, monster[i].CenterPoint.position);
				if (monsterDistance < distance)
				{
					distance = monsterDistance;
					select = monster[i];
				}
			}
			return select;
		}
	}

	// 범위 내 몬스터 랜덤 탐색
	public EnemyControl FindRandomTargetInRange(Vector3 Min, Vector3 Max)
	{
		var monster = MonsterList;

		if (monster.Count <= 0)
		{
			return null;
		}

		bool ExistMonster = false;
		EnemyControl select = monster[0];
		int StartIndex = UnityEngine.Random.Range(0, monster.Count);
		for (int i = 0; i < monster.Count; i++)
		{
			int CurrentIndex = (StartIndex + i) % monster.Count;
			// 범위 체크
			if ((monster[CurrentIndex].CenterPoint.position.x >= Min.x && monster[CurrentIndex].CenterPoint.position.x <= Max.x)
				&& (monster[CurrentIndex].CenterPoint.position.y >= Min.y && monster[CurrentIndex].CenterPoint.position.y <= Max.y))
			{
				select = monster[CurrentIndex];
				ExistMonster = true;
				break;
			}
		}

		if (ExistMonster == false)
			return null;
		else
			return select;
	}

	private float GetCustomDistance(Vector2 Vec1, Vector2 Vec2)
	{
		float width = Vec2.x - Vec1.x;
		float height = (Vec2.y - Vec1.y);
		return width * width + height * height; //어차피 거리비교를 위한것이므로 제곱으로 return
	}

	//소환시 타겟 일괄 전환
	public void ChangeTarget_WhenHeroSummon()
	{
		if (MonsterList.Count <= 0) { return; }

		int count = MonsterList.Count;
		EnemyControl enemy;

		for (int i = 0; i < count; i++)
		{
			enemy = MonsterList[i];
			enemy.Target = null;
			enemy.ChangeState(EActType.Idle);
		}
	}

	//사망시 타겟을 일괄 전환
	public void ChangeTarget_WhenHeroDie(HeroControl hero)
	{
		if (MonsterList.Count <= 0) { return; }

		int count = MonsterList.Count;
		EnemyControl enemy;

		for (int i = 0; i < count; i++)
		{
			enemy = MonsterList[i];
			if (enemy.Target == hero)
			{
				enemy.Target = null;
				enemy.ChangeState(EActType.Idle);
			}
		}
	}

	public List<EnemyControl> GetMonsterList()
	{
		return MonsterList;
	}

	// 남아있는 적 수 반환
	public int GetMonsterCount()
	{
		return MonsterList.Count;
	}

	//Wave 정보에 맞게 적을 스폰
	//private IEnumerator SpawnAllWaves()
	//{
	//	// 모든 웨이브를 순서대로 진행
	//	while (_currentWaveIndex < waves.Length)
	//	{
	//		WaveData currentWave = waves[_currentWaveIndex];
	//		yield return StartCoroutine(SpawnWave(currentWave));
	//		_currentWaveIndex++;
	//	}

	//	Debug.Log("모든 웨이브가 종료되었습니다!");
	//}

	//private IEnumerator SpawnWave(WaveData wave)
	//{
	//	Debug.Log($"--- {wave.waveName} 웨이브 시작 ---");

	//	for (int i = 0; i < wave.enemyCount; i++)
	//	{
	//		var newEnemy = EnemyPoolManager.Instance.Pop(EPoolType.Enemy);
	//		var enemyComp = newEnemy.GetComponent<EnemyControl>();
	//		var enemyType = enemyComp.EnemyType;

	//		// 몬스터 위치 및 인덱스 설정
	//		if (enemyType == EEnemyType.StageMonster)
	//		{
	//			enemyComp.transform.position = GetSpawnPosition(wave.spawnPattern, wave.patternRadius, i, wave.enemyCount);
	//		}
	//		else if (enemyType == EEnemyType.Boss)
	//		{
	//			enemyComp.transform.position = PositionInfo.Instance.BossPos.position;
	//		}


	//		//enemyComp.InitEnemy();
	//		MonsterList.Add(enemyComp);

	//		// 다음 적 소환까지 대기
	//		yield return new WaitForSeconds(wave.timeBetweenSpawns);
	//	}

	//	Debug.Log($"--- {wave.waveName} 웨이브 종료 ---");

	//	// 다음 웨이브까지 대기
	//	yield return new WaitForSeconds(wave.waveDelay);
	//}

	

	public void SpawnEnemy(ESpawnPattern spawnPattern, float patternRadius, int enemyCount)
	{
        var newEnemy = EnemyPoolManager.Instance.Pop(EPoolType.Enemy);
        var enemyComp = newEnemy.GetComponent<EnemyControl>();
        var enemyType = enemyComp.Info.EnemyType;

        for (int i = 0; i < enemyCount; i++)
		{
			// 몬스터 위치 및 인덱스 설정
			if (enemyType == EEnemyType.StageMonster)
			{
				enemyComp.transform.position = GetSpawnPosition(spawnPattern, patternRadius, i, enemyCount);
			}
			else if (enemyType == EEnemyType.Boss)
			{
				enemyComp.transform.position = PositionInfo.Instance.BossPos.position;
			}

            enemyComp.Init();
            MonsterList.Add(enemyComp);
			EnemyGenEvent.RaiseEvent(); // 몬스터 생성 이벤트 발생
        } 
    }

    private Vector3 GetSpawnPosition(ESpawnPattern pattern, float radius, int spawnIndex, int totalSpawns)
    {
        Vector3 pos = PositionInfo.Instance.StageCenter.position; // 기본 위치는 맵 중심

        switch (pattern)
        {
            case ESpawnPattern.Circle:
                float angle = spawnIndex * (360f / totalSpawns);
                pos += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
                break;

            case ESpawnPattern.Random:
                Vector2 randomCircle = Random.insideUnitCircle.normalized * radius; 
                pos += new Vector3(randomCircle.x, 0, randomCircle.y);
                break;

            case ESpawnPattern.Line:
                float startX = pos.x - radius;
                float endX = pos.x + radius;
                float z = pos.z + radius;
                float t = totalSpawns > 1 ? (float)spawnIndex / (totalSpawns - 1) : 0.5f;
                pos = new Vector3(Mathf.Lerp(startX, endX, t), pos.y, z);
                break;

            // 필요시 다른 패턴 추가
            default:
                break;
        }
        return pos;
    }

    // 적 처치(사망처리, 보상)
    public void EnemyDeath(EnemyControl m)
    {
        MonsterList.Remove(m);
    }
}

