using UnityEngine;

public class FXPoolManager : PoolManager<FXPoolManager, EFXPoolType>
{
    public GameObject PopDamageText(Vector3 position, AttackInfo attackInfo)
    {
        var obj = Pop(EFXPoolType.DamageText);
        obj.transform.position = position;
        obj.GetComponent<DamageFloating>().SetDamage(attackInfo);
        return obj;
    }
}
