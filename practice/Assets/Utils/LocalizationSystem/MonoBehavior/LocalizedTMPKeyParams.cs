using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LocalizedTMPKeyParams : LocalizedTMPBase
{
    public string Key;
    public FormatIndexedPointColor[] PointColors = null;
    public List<LocalizedParameter> Parameters = new List<LocalizedParameter>();
        

    public void SetPointColor(params (int, Color)[] pointColors)
    {
        PointColors = new FormatIndexedPointColor[pointColors.Length];
        for (int i = 0; i < pointColors.Length; i++)
        {
            var (index, color) = pointColors[i];
            PointColors[i] = new FormatIndexedPointColor { Index = index, Color = color };
        }
    }

    public void UpdateKey(string key)
    {
        SetKey(key);
        UpdateText();
    }

    public void UpdateKeyParameters(string key, params (bool, string)[] parameters)
    {
        SetKey(key);
        SetParameters(parameters);
        UpdateText();
    }

    public void UpdateKeyParametersAllKeys(string key, params string[] parameters)
    {
        SetKey(key);
        SetParametersAsKey(parameters);
        UpdateText();
    }

    public void UpdateKeyParametersAllValues(string key, params object[] parameters)
    {
        SetKey(key);
        SetParametersAsValue(parameters);
        UpdateText();
    }

    public void UpdateParameters(params (bool, string)[] parameters) 
    {
        SetParameters(parameters);
        UpdateText();
    }

    public void UpdateParametersAllKeys(params string[] parameters)
    {
        SetParametersAsKey(parameters);
        UpdateText();
    }

    public void UpdateParametersAllValues(params object[] parameters)
    {
        SetParametersAsValue(parameters);
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

        if (Parameters.Count == 0)
        {
            //Debug.Log($"[!] <{GetType()}> Error. Null Parameters.");
            TMP.SetText(Dictionary.Translate(Key));
            return;
        }

        var parameterStrings = new List<string>();
        for (int i = 0; i < Parameters.Count; i++)
        {
            string parameterText 
                = Parameters[i].IsKey == true 
                ? Dictionary.Translate(Parameters[i].Value) 
                : Parameters[i].Value;

            if (PointColors != null && PointColors.Length > 0)
            {
                var data = PointColors.FirstOrDefault(_ => _.Index == i);
                if (data != null)
                {
                    parameterText = $"<color=#{ColorUtility.ToHtmlStringRGBA(data.Color)}>{parameterText}</color>";
                }
            }

            parameterStrings.Add(parameterText);
        }

        TMP.SetText(Dictionary.Translate(Key, parameterStrings.ToArray()));
    }


    private void SetKey(string key)
    {
        Key = key;
    }

    private void SetParameters(params (bool, string)[] parameters)
    {
        Parameters.Clear();
        foreach (var (isKey, value) in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = isKey, Value = value });
        }
    }

    private void SetParametersAsKey(params object[] parameters)
    {
        Parameters.Clear();
        foreach (var obj in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = true, Value = obj.ToString() });
        }
    }
        
    private void SetParametersAsValue(params object[] parameters)
    {
        Parameters.Clear();
        foreach (var obj in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = false, Value = obj.ToString() });
        }
    }

}