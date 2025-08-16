using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enerygyText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI crystalText;

    private PlayerValue playerValue;

    private void OnEnable()
    {
        EventManager.Subscribe(EEventType.DataLoaded, OnDataLoaded);
        EventManager.Subscribe(EEventType.EnergyUpdated, UpdateEnergyText);
        EventManager.Subscribe(EEventType.GoldUpdated, UpdateGoldText);
        EventManager.Subscribe(EEventType.CrystalUpdated, UpdateCrystalText);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EEventType.DataLoaded, OnDataLoaded);
        EventManager.Unsubscribe(EEventType.EnergyUpdated, UpdateEnergyText);
        EventManager.Unsubscribe(EEventType.GoldUpdated, UpdateGoldText);
        EventManager.Unsubscribe(EEventType.CrystalUpdated, UpdateCrystalText);
    }

    private void OnDataLoaded()
    {
        playerValue = DataBase.PlayerData.Value;

        UpdateEnergyText();
        UpdateGoldText();
        UpdateCrystalText();
    }

    private void UpdateEnergyText()
    {
        enerygyText.text = $"{playerValue.CurrentEnergy}/{playerValue.MaxEnergy}";
    }
    private void UpdateGoldText()
    {
        string finalStr = playerValue.Gold.ToBCD();

        TextRoller.Roll(
        host: this,
        text: goldText,
        finalText: finalStr,
        mode: TextRoller.Mode.CountUp
        );
    }
    private void UpdateCrystalText()
    {
        string finalStr = playerValue.Crystal.ToBCD();

        TextRoller.Roll(
        host: this,
        text: crystalText,
        finalText: finalStr,
        mode: TextRoller.Mode.CountUp
        );
    }
}
