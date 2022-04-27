using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bite : Ability
{
    public int maxDamage;
    public int minDamage;

    [Range(0, 100)]
    public int hitChance;

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

            if (Utility.GetRandomChance(hitChance))
            {
                int damage = Utility.GetRandomValue(minDamage, maxDamage);

                target.Damage(damage);

                CombatLog.instance.CreateLog($"{target.unitName} took {damage} damage!");

                target.AddStatusEffect(statusEffect);
            }
            else
            {
                CombatLog.instance.CreateLog("Missed!");
            }

            TurnBasedCombatSystem.instance.EndTurn();
        }
    }
}