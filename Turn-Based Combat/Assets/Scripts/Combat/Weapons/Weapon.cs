using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public string weaponName;

    public int maxDamage;
    public int minDamage;

    public int Damage()
    {
        return Utility.GetRandomValue(minDamage, maxDamage);
    }
}