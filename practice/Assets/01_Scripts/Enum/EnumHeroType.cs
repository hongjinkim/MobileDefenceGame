using GoogleSheet.Core.Type;


[UGS(typeof(EHeroClassType))]
public enum EHeroClassType
{
    Warrior,
    Mage,
    Ranger,
    Supporter
}

[UGS(typeof(EHeroElementType))]
public enum EHeroElementType
{
    Ice,
    Fire,
    Thunder,
    Wind,
    Special
}