using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Revive : Ability
{
    public int amount;

    public override void SetMode()
    {
        TurnBasedCombatSystem.instance.AbilityMode(this, Team.Player);
    }

    public override void Activate(Unit target)
    {
        if (target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
            TurnBasedCombatSystem.instance.currentUnit.SetStamina(cost);

            target.Revive();
            target.Heal(amount);

            CombatLog.instance.CreateLog($"Revived {target.unitName} and healed for {amount} health!");

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}