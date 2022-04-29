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

    public override void Activate(Unit unit, Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
            TurnBasedCombatSystem.instance.GetCurrentUnit().SetStamina(cost);

            CombatLog.instance.CreateLog($"{unit.unitName} growled at {target.unitName}!", unit.team);

            target.AddStatusEffect(statusEffect);

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}