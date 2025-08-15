using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class PlayerValue : BaseSaveData<PlayerValue>
{
    public int MaxEnergy = 30; // 최대 에너지
    public int CurrentEnergy = 5; // 현재 에너지
    public BigNum Gold = 500; // 골드
    public BigNum Crystal = 50f; // 크리스탈
}
