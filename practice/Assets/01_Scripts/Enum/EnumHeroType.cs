using GoogleSheet.Core.Type;


[UGS(typeof(EHeroType))]
public enum EHeroType
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