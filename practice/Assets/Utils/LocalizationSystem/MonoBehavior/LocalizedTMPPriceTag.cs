using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizedTMPPriceTag : LocalizedTMPBase
{
    [SerializeField]
    private List<LocalizedPriceData> Prices;

    [SerializeField]
    private LocalizedPriceData Fallback;

    [SerializeField]
    private Color PricePointColor = Color.white;


    public override void UpdateText()
    {
        base.UpdateText();

        ELanguage language = Dictionary.CurrentLanguage;

        var data = Prices.FirstOrDefault(_ => _.Language == language) ?? Fallback;
        if (data != null)
        {
            TMP.SetText($"{data.Preffix}<color=#{ColorUtility.ToHtmlStringRGBA(PricePointColor)}>{data.PriceString}</color>{data.Suffix}");
        }
        else
        {
            TMP.SetText(string.Empty);
        }
    }

    public void AddPriceData(LocalizedPriceData data)
    {
        if (Prices.Exists(_ => _.Language == data.Language) == true)
        {
            Prices.RemoveAll(_ => _.Language == data.Language);
        }

        Prices.Add(data);
    }

    public void SetPriceData(LocalizedPriceData[] prices)
    {
        foreach (LocalizedPriceData price in prices)
        {
            AddPriceData(price);
        }
    }

    public void SetFallback(LocalizedPriceData fallback)
    {
        Fallback = fallback;
    }

    public void SetPointColor(Color color)
    {
        PricePointColor = color;
        UpdateText();
    }
}

[Serializable]
public class LocalizedPriceData
{
    public ELanguage Language;
    public string Preffix;
    public string PriceString;
    public string Suffix;
}
