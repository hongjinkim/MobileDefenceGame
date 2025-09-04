using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LocalizedTMPFormatParams : LocalizedTMPBase
{
    public string Format;
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
        UpdateText();
    }

    public void UpdateFormat(string format)
    {
        SetFormat(format);
        UpdateText();
    }

    public void UpdateFormatParameters(string format, params (bool, string)[] parameters)
    {
        SetFormat(format);
        SetParameters(parameters);
        UpdateText();
    }

    public void UpdateFormatParametersAllKeys(string format, params string[] parameters)
    {
        SetFormat(format);
        SetParametersAsKeys(parameters);
        UpdateText();
    }

    public void UpdateFormatParametersAllValues(string key, params object[] parameters)
    {
        SetFormat(key);
        SetParametersAsValues(parameters);
        UpdateText();
    }


    public void UpdateParameters(params (bool, string)[] parameters)
    {
        SetParameters(parameters);
        UpdateText();
    }
    public void UpdateParametersAllKeys(params string[] parameters)
    {
        SetParametersAsKeys(parameters);
        UpdateText();
    }

    public void UpdateParametersAllValues(params object[] parameters)
    {
        SetParametersAsValues(parameters);
        UpdateText();
    }

    public override void UpdateText()
    {
        base.UpdateText();

        if (string.IsNullOrWhiteSpace(Format) == true)
        {
            TMP.SetText("");
            Debug.Log($"[!] <{GetType()}> Format is Empty. Not recommended.", gameObject);
            return;
        }

        if (Parameters.Count == 0)
        {
            //Debug.Log($"[!] <{GetType()}> Error. Null Parameters.");
            TMP.SetText(Format);
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

        TMP.SetText(string.Format(Format, parameterStrings.ToArray()));
    }


    private void SetFormat(string format) => Format = format;

    private void SetParameters(params (bool, string)[] parameters)
    {
        Parameters.Clear();
        foreach (var (isKey, value) in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = isKey, Value = value });
        }
    }

    private void SetParametersAsKeys(params string[] parameters)
    {
        Parameters.Clear();
        foreach (var text in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = true, Value = text });
        }
    }

    private void SetParametersAsValues(params object[] parameters)
    {
        Parameters.Clear();
        foreach (var obj in parameters)
        {
            Parameters.Add(new LocalizedParameter { IsKey = false, Value = obj.ToString() });
        }
    }

}

