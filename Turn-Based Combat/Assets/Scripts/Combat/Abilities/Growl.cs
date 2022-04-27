using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Growl : Ability
{
    public StatusEffect statusEffect;

    public override void SetMode()
    {
        TurnBasedCombatSystem.instance.AbilityMode(this, Team.Enemy);
    }

    public override void Activate(Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
            TurnBasedCombatSystem.instance.GetCurrentUnit().SetStamina(cost);

            target.AddStatusEffect(statusEffect);

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}