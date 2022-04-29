using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Heal : Ability
{
    public int amount;

    public override void SetMode()
    {
        TurnBasedCombatSystem.instance.AbilityMode(this, Team.Player);
    }

    public override void Activate(Unit unit, Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
            TurnBasedCombatSystem.instance.GetCurrentUnit().SetStamina(cost);

            target.Heal(amount);

            CombatLog.instance.CreateLog($"{unit.unitName} healed {target.unitName} for {amount} health!", unit.team);

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}