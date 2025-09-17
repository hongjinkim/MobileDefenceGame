using GoogleSheet.Core.Type;


[UGS(typeof(EHeroClassType))]
public enum EHeroClassType
{
    Warrior,
    Mage,
    Assassin,
    Support
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