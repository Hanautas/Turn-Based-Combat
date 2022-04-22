using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bleed : StatusEffect
{
    public int damage;

    public override void Activate(Unit target)
    {
        target.Damage(damage);

        CombatLog.instance.CreateLog($"{target.unitName} took {damage} bleed damage!");
    }
}