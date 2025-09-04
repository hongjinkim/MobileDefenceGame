
using UnityEngine;


public class LocalizedTMPSystemFont : LocalizedTMPBase
{
    [SerializeField]
    protected string Key;

    private void Awake()
    {
            
    }

    public void UpdateKey(string key)
    {
        Key = key;
        UpdateText();
    }

    public override void UpdateText()
    {
        if (string.IsNullOrWhiteSpace(Key) == true)
        {
            TMP.SetText("");
            Debug.Log($"[!] <{GetType()}> Key is Empty. Not recommended.", gameObject);
            return;
        }

        TMP.SetText(Dictionary.Translate(Key));
    }

    public bool CheckKey(string key) => Key == key;
}
