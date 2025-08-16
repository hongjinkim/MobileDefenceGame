using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enerygyText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI crystalText;

    private void OnEnable()
    {
        EventManager.Subscribe(EEventType.DataLoaded, OnDataLoaded);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EEventType.DataLoaded, OnDataLoaded);
    }

    private void OnDataLoaded()
    {
        UpdateEnergyText();
        UpdateGoldText();
        UpdateCrystalText();
    }

    private void UpdateEnergyText()
    {
        enerygyText.text = $"{DataBase.PlayerData.Value.CurrentEnergy}/{DataBase.PlayerData.Value.MaxEnergy}";
    }
    private void UpdateGoldText()
    {
        goldText.text = DataBase.PlayerData.Value.Gold.ToBCD();
    }
    private void UpdateCrystalText()
    {
        crystalText.text = DataBase.PlayerData.Value.Crystal.ToBCD();
    }
}
