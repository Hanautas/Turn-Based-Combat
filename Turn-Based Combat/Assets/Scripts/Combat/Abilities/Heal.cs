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

    public override void Activate(Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();

            target.Heal(amount);

            CombatLog.instance.CreateLog($"Healed {target.unitName} for {amount} health!");

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}