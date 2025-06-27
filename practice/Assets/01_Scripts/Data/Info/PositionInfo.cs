using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PositionInfo : BasicSingleton<PositionInfo>
{
	public Transform MapCenter;
	public Transform BossPos;
	public Transform[] HeroPos;

	private List<Transform> transformList = new List<Transform>();

	private void Awake()
	{
		
	}

	public List<Transform> GetAllTransform()
	{
		transformList.Add(MapCenter);
		transformList.Add(BossPos);
		transformList.AddRange(HeroPos.ToList());
		return transformList;
	}
	
}

