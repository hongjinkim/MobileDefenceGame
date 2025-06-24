using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

// 몬스터 및 보스의 관리
public class EnemyManager : BasicSingleton<EnemyManager>
{
	[Header("이벤트")]
	[SerializeField] private VoidEventChannelSO monsterGen;

	private List<EnemyControl> MonsterList = new List<EnemyControl>();



	private PlayerData Player => DataManager.GetPlayerData();
	private StageData Stage => DataManager.GetStageData();

	

	// N 스테이지 시작
	private void StartStage()
	{
		EnemyClearAll();
		StopAllCoroutines();
		//StartCoroutine(SpawnStageEnemy(EEnemyType.StageMonster));
	}

	// 해당 스테이지 적 초기화
	private void StageEnemyInit(int StageNum)
	{
		
	}

	// 스테이지 적 스폰
	//IEnumerator SpawnStageEnemy()
	//{
		
	//}

	// 몬스터 젠
	private IEnumerator GenStageMonster()
	{
		yield return new WaitForSeconds(1);
	}

	// 보스 몬스터 젠
	private void GenStageBoss()
	{
		
	}

	// 스테이지 적 소환
	private void EnemySummon()
	{
		
	}

	/************************************************************/

	//전체 사망시 적 공격행위 중단
	private void EnemyStopAttack()
	{
		for (int i = 0; i < MonsterList.Count; i++)
		{
			//MonsterList[i].StopAttackWhenHeroDie();
		}
	}

	// 적 전부 삭제
	private void EnemyClearAll()
	{
		for (int i = 0; i < MonsterList.Count; i++)
		{
			EnemyPoolManager.Instance.Push(MonsterList[i].gameObject, EPoolType.Monster);
		}
		MonsterList.Clear();
	}

	// 적 처치(사망처리, 보상)
	public void EnemyDeath(EnemyControl m)
	{
		MonsterList.Remove(m);
	}

	private void SetPosition(EnemyControl m)
	{
		
	}

	// 적 보상
	public void Reward()
	{
		
	}


	// 대상과 가장 가까운 몬스터를 탐색
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
}

