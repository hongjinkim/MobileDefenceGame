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
	public float patternRadius = 5f;

	private List<EnemyControl> MonsterList = new List<EnemyControl>();

	private int _currentStage;
	[SerializeField] private EnemyControl bossEnemy; // 보스 몬스터

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
		_currentStage = PlayerManager.Instance.CurrentStage;
        //waves = Stage.GetWaveData();
    }

	public void ClearAllEnemy()
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

	public void SpawnEnemy(ESpawnPattern spawnPattern, int enemyCount, StageEnemyValue enemyInfo, string enemyID = null)
	{
        for (int i = 0; i < enemyCount; i++)
		{
            var newEnemy = EnemyPoolManager.Instance.Pop(EPoolType.Enemy);
            var enemyComp = newEnemy.GetComponent<EnemyControl>();
            var enemyType = enemyComp.Info.EnemyType;
            

            // 몬스터 위치 및 인덱스 설정
            if (enemyType == EEnemyType.StageMonster)
			{
				enemyComp.transform.position = GetSpawnPosition(spawnPattern, patternRadius, i, enemyCount);
			}

            enemyComp.Init(enemyInfo);
            MonsterList.Add(enemyComp);
			EventManager.Raise(EEventType.EnemyGenerated); // 몬스터 생성 이벤트 발생
        } 
    }
	public void SpawnBoss(StageEnemyValue enemyInfo, string enemyID = null)
	{
        var newEnemy = EnemyPoolManager.Instance.Pop(EPoolType.Enemy);
        bossEnemy = newEnemy.GetComponent<EnemyControl>();
		bossEnemy.Info.EnemyType = EEnemyType.Boss;
        bossEnemy.MarkAsLastBoss(false);
        var enemyType = bossEnemy.Info.EnemyType;

        // 몬스터 위치 및 인덱스 설정
		bossEnemy.transform.position = PositionInfo.Instance.BossPos.position;
        

        bossEnemy.Init(enemyInfo);
        MonsterList.Add(bossEnemy);
        EventManager.Raise(EEventType.EnemyGenerated); // 몬스터 생성 이벤트 발생
    }
    public void SpawnLastBoss(StageEnemyValue enemyInfo, string enemyID = null)
    {
        var newEnemy = EnemyPoolManager.Instance.Pop(EPoolType.Enemy);
        bossEnemy = newEnemy.GetComponent<EnemyControl>();
        bossEnemy.Info.EnemyType = EEnemyType.Boss;
		bossEnemy.MarkAsLastBoss(true); // 마지막 보스로 표시
        var enemyType = bossEnemy.Info.EnemyType;

        // 몬스터 위치 및 인덱스 설정
        bossEnemy.transform.position = PositionInfo.Instance.BossPos.position;


        bossEnemy.Init(enemyInfo);
        MonsterList.Add(bossEnemy);
        EventManager.Raise(EEventType.EnemyGenerated); // 몬스터 생성 이벤트 발생
    }

    private Vector3 GetSpawnPosition(ESpawnPattern pattern, float radius, int spawnIndex, int totalSpawns)
    {
        Vector3 center = PositionInfo.Instance.StageCenter.position; // 기본 위치는 맵 중심
        Vector3 pos = center;

        if (totalSpawns <= 0) return pos;
        float twoPI = Mathf.PI * 2f;

        switch (pattern)
        {
            case ESpawnPattern.Circle:
            {
                float angle = spawnIndex * (360f / totalSpawns);
                pos += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
                break;
            }
                
            case ESpawnPattern.Random:
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * radius;
                pos += new Vector3(randomCircle.x, 0, randomCircle.y);
                break;
            }
                
            case ESpawnPattern.Line:
            {
                float startX = pos.x - radius;
                float endX = pos.x + radius;
                float z = pos.z + radius;
                float t = totalSpawns > 1 ? (float)spawnIndex / (totalSpawns - 1) : 0.5f;
                pos = new Vector3(Mathf.Lerp(startX, endX, t), pos.y, z);
                break;
            }
                
            case ESpawnPattern.RandomInsideDisk:
            {
                // 원판 내부 균일 샘플링: r = R * sqrt(u)
                float u = Random.value;
                float r = radius * Mathf.Sqrt(u);
                float a = Random.value * twoPI;
                pos += new Vector3(Mathf.Cos(a) * r, 0, Mathf.Sin(a) * r);
                break;
            }

            case ESpawnPattern.SemiCircleForward:
            {
                // -90° ~ +90° (월드 +Z 전방 기준)
                float t = totalSpawns > 1 ? (float)spawnIndex / (totalSpawns - 1) : 0.5f;
                float angle = Mathf.Lerp(-90f, 90f, t);
                pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);
                break;
            }

            case ESpawnPattern.SemiCircleBackward:
            {
                // +90° ~ +270° (월드 -Z 후방 기준)
                float t = totalSpawns > 1 ? (float)spawnIndex / (totalSpawns - 1) : 0.5f;
                float angle = Mathf.Lerp(90f, 270f, t);
                pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);
                break;
            }

            case ESpawnPattern.Arc120:
            {
                // 전방 중심 120도 부채꼴: -60° ~ +60°
                float t = totalSpawns > 1 ? (float)spawnIndex / (totalSpawns - 1) : 0.5f;
                float angle = Mathf.Lerp(-60f, 60f, t);
                pos += new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);
                break;
            }

            case ESpawnPattern.SpiralOut:
            {
                // 2바퀴 돌며 바깥으로: 각도는 진행비율*turns*360, 반경은 진행비율*R
                float turns = 2f;
                float t = (spawnIndex + 0.5f) / Mathf.Max(1, totalSpawns);
                float angle = t * turns * twoPI;
                float r = Mathf.Lerp(radius * 0.2f, radius, t);
                pos += new Vector3(Mathf.Cos(angle) * r, 0, Mathf.Sin(angle) * r);
                break;
            }

            case ESpawnPattern.Phyllotaxis:
            {
                // 골든 앵글 분포(원판에 균일하게 퍼짐)
                const float goldenAngle = 137.50776405003785f * Mathf.Deg2Rad;
                float n = spawnIndex + 0.5f;
                float t = n / Mathf.Max(1, totalSpawns);
                float a = n * goldenAngle;
                float r = radius * Mathf.Sqrt(t);
                pos += new Vector3(Mathf.Cos(a) * r, 0, Mathf.Sin(a) * r);
                break;
            }

            case ESpawnPattern.Rings2:
            {
                // 내외 2개의 반지: 40%는 내반지, 나머지 외반지
                int innerCount = Mathf.Clamp(Mathf.CeilToInt(totalSpawns * 0.4f), 1, totalSpawns - 1);
                int outerCount = Mathf.Max(1, totalSpawns - innerCount);
                if (spawnIndex < innerCount)
                {
                    float angle = (spawnIndex / (float)innerCount) * 360f;
                    float r = radius * 0.6f;
                    pos += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * r, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * r);
                }
                else
                {
                    int i = spawnIndex - innerCount;
                    float angle = (i / (float)outerCount) * 360f;
                    float r = radius;
                    pos += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * r, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * r);
                }
                break;
            }

            case ESpawnPattern.Cluster3:
            {
                // 3개 군집 중심을 원 둘레에 배치, 각 군집은 작은 원에 분포
                int clusters = 3;
                int perCluster = Mathf.CeilToInt(totalSpawns / (float)clusters);
                int clusterIdx = spawnIndex % clusters;
                int orderInCluster = spawnIndex / clusters;

                float clusterAngle = clusterIdx * (360f / clusters);
                Vector3 clusterCenter = center + new Vector3(
                    Mathf.Cos(clusterAngle * Mathf.Deg2Rad),
                    0,
                    Mathf.Sin(clusterAngle * Mathf.Deg2Rad)
                ) * (radius * 0.8f);

                float subAngle = (orderInCluster / Mathf.Max(1f, perCluster)) * 360f;
                float subR = radius * 0.22f;
                pos = clusterCenter + new Vector3(
                    Mathf.Cos(subAngle * Mathf.Deg2Rad) * subR, 0,
                    Mathf.Sin(subAngle * Mathf.Deg2Rad) * subR
                );
                break;
            }

            case ESpawnPattern.Lanes4:
            {
                // 상/하/좌/우 4개의 레인으로 번갈아가며 밀도 있게
                int lanes = 4;
                int lane = spawnIndex % lanes;
                int order = spawnIndex / lanes;
                int perLane = Mathf.CeilToInt(totalSpawns / (float)lanes);
                float d = perLane > 1 ? order / (float)(perLane - 1) : 0.5f;
                d = Mathf.Clamp01(d);
                float dist = Mathf.Lerp(radius * 0.25f, radius, d);

                Vector3 dir = lane switch
                {
                    0 => Vector3.forward,   // +Z
                    1 => Vector3.right,     // +X
                    2 => Vector3.back,      // -Z
                    _ => Vector3.left       // -X
                };
                pos += dir * dist;
                break;
            }

            case ESpawnPattern.Ellipse:
            {
                // 가로로 늘어진 타원
                float xR = radius * 1.6f;
                float zR = radius * 0.9f;
                float angle = spawnIndex * (360f / Mathf.Max(1, totalSpawns)) * Mathf.Deg2Rad;
                pos += new Vector3(Mathf.Cos(angle) * xR, 0, Mathf.Sin(angle) * zR);
                break;
            }

            case ESpawnPattern.NoisyCircle:
            {
                // 원 둘레 + 약간의 노이즈
                float baseAngle = spawnIndex * (360f / Mathf.Max(1, totalSpawns));
                // 간단한 해시 기반 시드
                float seed = Mathf.Abs(Mathf.Sin((spawnIndex + 1) * 12.9898f) * 43758.5453f);
                float noise = (seed - Mathf.Floor(seed));      // 0~1
                float jitter = Mathf.Lerp(-0.2f, 0.2f, noise); // -0.2~0.2
                float r = radius * (1f + jitter * 0.5f);       // 반경에 10% 내외 흔들림
                float a = baseAngle * Mathf.Deg2Rad;
                pos += new Vector3(Mathf.Cos(a) * r, 0, Mathf.Sin(a) * r);
                break;
            }

            case ESpawnPattern.Grid:
            {
                // 정사각 격자에 가득 채우기(반지름을 반쪽 폭으로 간주)
                int grid = Mathf.CeilToInt(Mathf.Sqrt(totalSpawns));
                float width = radius * 2f;
                float step = grid > 1 ? width / (grid - 1) : 0f;
                int row = spawnIndex / grid;
                int col = spawnIndex % grid;
                float x = -radius + col * step;
                float z = -radius + row * step;
                pos += new Vector3(x, 0, z);
                break;
            }

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

