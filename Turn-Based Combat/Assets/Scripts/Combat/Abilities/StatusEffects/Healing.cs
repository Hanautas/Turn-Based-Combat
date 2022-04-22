using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Healing : StatusEffect
{
    public int amount;

    public override void Activate(Unit target)
    {
        target.Heal(amount);

        CombatLog.instance.CreateLog($"{target.unitName} healed for {amount} health!");
    }
}