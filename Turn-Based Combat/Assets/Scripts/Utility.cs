using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static int GetRandomValue(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static bool GetRandomChance(int chance)
    {
        int result = Random.Range(0, 100);

        Debug.Log($"Chance: {chance} / Result: {result}");

        if (chance <= result)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}