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

    public override void Activate(Unit target)
    {
        if (!target.IsDead())
        {
            TurnBasedCombatSystem.instance.ResetAbilityMode();
            TurnBasedCombatSystem.instance.GetCurrentUnit().SetStamina(cost);

            coroutineStarter.StartCoroutine(StartAttack(target));
        }
    }

    private IEnumerator StartAttack(Unit target)
    {
        int damage = Utility.GetRandomValue(minDamage, maxDamage);

        target.Damage(damage);

        CombatLog.instance.CreateLog($"{target.unitName} took {damage} damage!");

        for (int i = 0; i < hits; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (Utility.GetRandomChance(hitChance))
            {
                int damage_2 = Utility.GetRandomValue(minDamage, maxDamage);

                Unit target_2 = TurnBasedCombatSystem.instance.GetRandomEnemyUnit();

                target_2.Damage(damage_2);

                CombatLog.instance.CreateLog($"{target_2.unitName} took {damage_2} damage!");
            }
            else
            {
                CombatLog.instance.CreateLog("Missed!");
            }
        }

        TurnBasedCombatSystem.instance.EndTurn();
    }
}