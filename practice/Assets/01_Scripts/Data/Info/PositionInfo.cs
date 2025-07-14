using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PositionInfo : BasicSingleton<PositionInfo>
{
	public Transform MinPos, MaxPos;
    public Transform StageCenter;
	public Transform LobbyCenter;
    public Transform BossPos;
	public Transform[] HeroPos;

	
}

