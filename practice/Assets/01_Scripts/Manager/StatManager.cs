using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


// 최종 능력치 및 각종 능력치 계산
public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; } = null;

    public static Action OnPowerChanged;

    private const string POWER_INCREASE = "<voffset=-0.5em><size=40><sprite=4></size></voffset>{0} <color=green> ▲{1}";
    private const string POWER_DECREASE = "<voffset=-0.5em><size=40><sprite=4></size></voffset>{0} <color=red> ▼-{1}";


    public BigNum FinalAttack { get; private set; }                         // 공격력_근거리
    public BigNum FinalAttack_Skill { get; private set; }                  // 공격력_스킬
    public BigNum FinalHealthPoint { get; private set; }                   // 체력
    public double FinalCritical { get; private set; }                       // 치명타
    public float FinalCriticalRate { get; private set; }                    // 치명타 확률
    public double FinalDefaultAttackDamage { get; private set; }            // 기본 공격 데미지
    public double FinalSkillDamage { get; private set; }                    // 스킬 데미지
    public double FinalSkillCritical { get; private set; }                  // 스킬 치명타
    public float FinalSkillCriticalRate { get; private set; }               // 스킬 치명타 확률
    public double FinalBossDamage { get; private set; }                     // 보스 데미지
    public float FinalAttackSpeed { get; private set; }                     // 공격속도
    public BigNum FinalPower { get; private set; }                           // 전투력


    private PlayerData Player => DataManager.GetPlayerData();
    private InitialData Initial => DataManager.GetInitialData();

    private BigNum tempTargetPower;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateStatAll();
    }
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    private void UpdateStatAll(bool isinit) => UpdateStatAll();
    private void UpdateStatAll(int index) => UpdateStatAll();
    // 최종 능력치들 계산
    // 업그레이드 진행 시, 이벤트로 호출
    private void UpdateStatAll()
    {
       
    }
    
}
