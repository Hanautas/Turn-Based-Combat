using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public string unitName;
    public Color unitNameColor = new Color(1, 1, 1, 1);

    [Header("Health")]
    public bool isDead;
    public int maxHealth;
    public int currentHealth;

    [Header("Stamina")]
    public int maxStamina;
    public int currentStamina;

    [Header("Damage")]
    public int damage;
}