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

    public override void Activate()
    {
        // Attack

        for (int i = 0; i < hits; i++)
        {
            if (GetRandomChance(hitChance))
            {
                // Attack
            }
        }
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