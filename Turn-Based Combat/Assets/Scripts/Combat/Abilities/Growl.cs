using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Growl : Ability
{
    public override void Activate(Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
        }
    }
}