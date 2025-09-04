using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;

public class LocalizedTMPConstant_Script : LocalizedTMPBase
{
    [SerializeField] protected string Key;
    [SerializeField] private float script_time = 1.5f;

    private Color fontColor;
    private AudioPlayerSingle ScriptSound;


    private string script;
    private int letterCount_total;
    private int letterCount;


    private float timer_per_letter;
    private float timer;
    private bool isStart;
    private StringBuilder SB = new StringBuilder();

    private void Awake()
    {
        ScriptSound = GetComponent<AudioPlayerSingle>();
        fontColor = GetComponent<TMP_Text>().color;
    }

    private void Update()
    {
        if (!isStart) { return; }

        timer += Time.deltaTime;

        if(timer > timer_per_letter)
        {
            ScriptSound.Play();
            TMP.SetText(SB.Append(script[letterCount]).ToString());
            letterCount++;

            if(letterCount < letterCount_total) { timer -= timer_per_letter; }
            else { enabled = false; }
        }
    }

    public void UpdateKey(string key)
    {
        Key = key;
        UpdateText();
    }
        
    public override void UpdateText()
    {
        base.UpdateText();

        if (string.IsNullOrWhiteSpace(Key) == true)
        {
            TMP.SetText("");
            Debug.Log($"[!] <{GetType()}> Key is Empty. Not recommended.", gameObject);
            return;
        }

        script = Dictionary.Translate(Key);
        letterCount_total = script.Length;
        letterCount = 0;

        timer_per_letter = script_time / letterCount_total;
        TMP.SetText(script.ToString());
        TMP.color = Color.clear;
        //TMP.SetText("");
        SB.Clear();
    }

    public void StartScripting()
    {
        isStart = false;
        enabled = true;
        StartCoroutine(InitScripting());
    }

    private IEnumerator InitScripting()
    {
        TMP.color = Color.clear;
        TMP.fontSizeMin = 0;
        TMP.enableAutoSizing = true;
        yield return null;
        float size = TMP.fontSize;
        yield return null;
        TMP.enableAutoSizing = false;
        TMP.fontSize = size;
        yield return null;
        TMP.SetText("");
        TMP.color = fontColor;
        isStart = true;
    }
    public bool CheckKey(string key) => Key == key;
}
