using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



// 유저 정보 관리
public class PlayerManager : BasicSingleton<PlayerManager>
{
    [Header("이벤트")]
    [SerializeField] private VoidEventChannelSO levelUp;

    private PlayerData playerData => DataBase.GetPlayerData();

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {

    }

    public void GainExp(BigNum GetExp)
    {
        
    }

}

