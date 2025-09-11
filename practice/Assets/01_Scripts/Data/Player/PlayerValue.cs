using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class PlayerValue : BaseSaveData<PlayerValue>
{
    public string NickName; // 닉네임
    public bool IsPrologue = false; // 프롤로그 진행 여부
    public int MaxEnergy = 30; // 최대 에너지
    public int CurrentEnergy = 0; // 현재 에너지
    public BigNum Gold = 500; // 골드
    public BigNum Crystal = 50f; // 크리스탈

    public void SetPrologue(bool prologue)
    {
        this.IsPrologue = prologue;
        Save();
    }

    public void UpdateEnergy(int amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > MaxEnergy) CurrentEnergy = MaxEnergy; // 최대 에너지 초과 방지
        if (CurrentEnergy < 0) CurrentEnergy = 0; // 에너지가 음수가 되지 않도록
        EventManager.Raise(EEventType.EnergyUpdated); // 에너지 업데이트 이벤트 발생
        Save();
    }
    public void UpdateGold(BigNum amount)
    {
        Gold += amount;
        if (Gold < 0) Gold = 0; // 골드가 음수가 되지 않도록
        EventManager.Raise(EEventType.GoldUpdated); // 골드 업데이트 이벤트 발생
        Save();
    }
    public void UpdateCrystal(BigNum amount)
    {
        Crystal += amount;
        if (Crystal < 0) Crystal = 0; // 크리스탈이 음수가 되지 않도록
        EventManager.Raise(EEventType.CrystalUpdated); // 크리스탈 업데이트 이벤트 발생
        Save();
    }
}
