using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Fury : Ability
{
    public int maxDamage;
    public int minDamage;

    public int hits;
    
    [Range(0, 100)]
    public int hitChance;

    private class ClassB : MonoBehaviour{}

    private ClassB coroutineStarterWorker;

    private ClassB coroutineStarter
    {
        get
        {
            if (coroutineStarterWorker != null)
            {
                return coroutineStarterWorker;
            }
            else
            {
                return InitializeCoroutineStarter();
            }
        }

        set{}
    }

    private ClassB InitializeCoroutineStarter()
    {
        GameObject instance = new GameObject();
        instance.isStatic = true;
        coroutineStarterWorker = instance.AddComponent<ClassB>();
        
        return coroutineStarterWorker;
    }

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

            coroutineStarter.StartCoroutine(StartAttack(unit, target));
        }
    }

    private IEnumerator StartAttack(Unit unit, Unit target)
    {
        int damage = Utility.GetRandomValue(minDamage, maxDamage);

        target.Damage(damage);

        CombatLog.instance.CreateLog($"{unit.unitName} attacked {target.unitName} for {damage} damage!", unit.team);

        for (int i = 0; i < hits; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (Utility.GetRandomChance(hitChance))
            {
                int damage_2 = Utility.GetRandomValue(minDamage, maxDamage);

                Unit target_2 = null;

                if (unit.team == Team.Player)
                {
                    target_2 = TurnBasedCombatSystem.instance.GetRandomEnemyUnit();
                }
                else if (unit.team == Team.Enemy)
                {
                    target_2 = TurnBasedCombatSystem.instance.GetRandomPlayerUnit();
                }

                target_2.Damage(damage_2);

                CombatLog.instance.CreateLog($"{unit.unitName} attacked {target_2.unitName} for {damage_2} damage!", unit.team);
            }
            else
            {
                CombatLog.instance.CreateLog($"{unit.unitName} missed!", unit.team);
            }
        }

        TurnBasedCombatSystem.instance.EndTurn();
    }
}