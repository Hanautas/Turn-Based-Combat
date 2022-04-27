using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public string weaponName;

    public int maxDamage;
    public int minDamage;

    public AudioClip soundEffect;

    public int Damage()
    {
        AudioManager.instance.PlaySound(soundEffect);

        return Utility.GetRandomValue(minDamage, maxDamage);
    }
}