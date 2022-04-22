using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Stun : StatusEffect
{
    public int rounds;

    public override void Activate(Unit target)
    {
        CombatLog.instance.CreateLog($"{target.unitName} is stunned and cannot attack!");

        TurnBasedCombatSystem.instance.EndTurn();
    }
}