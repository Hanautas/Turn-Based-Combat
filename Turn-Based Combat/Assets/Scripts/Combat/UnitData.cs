using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    [Header("Name")]
    public string unitName;
    public Color unitNameColor = new Color(1, 1, 1, 1);

    [Header("Health")]
    public int maxHealth;

    [Header("Stamina")]
    public int maxStamina;

    [Header("Damage")]
    public Weapon weapon;

    [Header("Abilities")]
    public Ability[] abilities;
}