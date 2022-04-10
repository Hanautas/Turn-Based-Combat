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

    public override void Activate()
    {
        // Attack
    }

    public int GetRandomDamage(int min, int max)
    {
        return Random.Range(min, max);
    }

    public bool GetRandomChance(int chance)
    {
        int result = Random.Range(0, 100);

        if (hitChance <= result)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}