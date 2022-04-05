using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Heal : Ability
{
    public int amount;

    public override void Activate()
    {
        TurnBasedCombatSystem.instance.target.Heal(amount);
    }
}