using System.Collections;
using UnityEngine;


public class HeroData
{
    public int HeroId { get; private set; }
    public ERarity HeroRarity { get; private set; }
    public string HeroName { get; private set; }
    public string HeroDescription { get; private set; }
    public Sprite HeroIcon { get; private set; }
    public int Level { get; private set; }
    public BigNum Exp { get; private set; }
    public BigNum MaxExp { get; private set; }
    
    public HeroData(DataTable.영웅 hero)
    {
        LoadData(hero);
    }

    private void LoadData(DataTable.영웅 hero)
    {
        HeroId = hero.영웅_ID;
        HeroRarity = hero.영웅_희귀도;
        HeroName = hero.영웅_이름;
        HeroDescription = hero.영웅_설명;
        //HeroIcon = Resources.Load<Sprite>($"Icons/Heroes/{hero.아이콘}");
        Level = hero.영웅_레벨;
        Exp = new BigNum(hero.영웅_경험치);
        MaxExp = new BigNum(hero.영웅_최대경험치);
    }
}
