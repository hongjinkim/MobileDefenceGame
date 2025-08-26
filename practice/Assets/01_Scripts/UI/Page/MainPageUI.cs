using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPageUI : MonoBehaviour
{

    [Header("UI �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI enerygyText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private TextMeshProUGUI stageNumText;
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI waveText;
    [Header("�κ� ����")]
    [SerializeField] private float scrollSpeed = 5f; // m/s
    [SerializeField] private Vector3 scrollAxis = new Vector3(0, 0, -1); // �ڷ� �帣�� ����
    [SerializeField] private float tileLength = 10f; // �� Ÿ���� ����(Z)
    [SerializeField] private int visibleTiles = 3;

    private Transform[] tiles;


    private PlayerValue playerValue;
    private int currentStage;
    private string stageName;

    private void Awake()
    {
        EventManager.Subscribe(EEventType.DataLoaded, OnDataLoaded);
        EventManager.Subscribe(EEventType.EnergyUpdated, UpdateEnergyText);
        EventManager.Subscribe(EEventType.GoldUpdated, UpdateGoldText);
        EventManager.Subscribe(EEventType.CrystalUpdated, UpdateCrystalText);
        EventManager.Subscribe(EEventType.StageCleared, UpdateStageClearText);
    }

    private void OnApplicationQuit()
    {
        EventManager.Unsubscribe(EEventType.DataLoaded, OnDataLoaded);
        EventManager.Unsubscribe(EEventType.EnergyUpdated, UpdateEnergyText);
        EventManager.Unsubscribe(EEventType.GoldUpdated, UpdateGoldText);
        EventManager.Unsubscribe(EEventType.CrystalUpdated, UpdateCrystalText);
        EventManager.Unsubscribe(EEventType.StageCleared, UpdateStageClearText);
    }

    private void OnDataLoaded()
    {
        Debug.Log("MainPageUI: Data Loaded");
        playerValue = GameDataManager.PlayerData.Value;
        currentStage = PlayerManager.Instance.CurrentStage;
        stageName = DataBase.TryGetStageName(
            currentStage,
            out string name
        ) ? name : "Unknown Stage";

        UpdateEnergyText();
        UpdateGoldText();
        UpdateCrystalText();
        UpdateStageClearText();
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

    private void UpdateStageClearText()
    {
        currentStage = PlayerManager.Instance.CurrentStage;
        stageName = DataBase.TryGetStageName(
            currentStage,
            out string name
        ) ? name : "Unknown Stage";

        stageNumText.text = $"{currentStage.ToString("D2")}.";
        stageNameText.text = stageName;
        waveText.text = $"0/20"; // Ŭ���� ���� �ް� ����
    }
}
