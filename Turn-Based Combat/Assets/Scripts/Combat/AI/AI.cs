using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private Unit unit;

    public AI(Unit newUnit)
    {
        unit = newUnit;
    }

    public void Activate()
    {
        if (Random.value > 0.5f)
        {
            List<Ability> abilityList = new List<Ability>(unit.abilities);

            while (abilityList.Count > 0)
            {
                Ability ability = GetRandomAbility(abilityList);

                if (unit.CheckStaminaCost(ability.cost))
                {
                    ability.Activate(TurnBasedCombatSystem.instance.GetRandomPlayerUnit());

                    return;
                }
                else
                {
                    abilityList.Remove(ability);
                }
            }
        }

        TurnBasedCombatSystem.instance.GetRandomPlayerUnit().Damage(unit.Attack());

        TurnBasedCombatSystem.instance.EndTurn();
    }

    private Ability GetRandomAbility(List<Ability> abilities)
    {
        return abilities[Random.Range(0, abilities.Count)];
    }
}