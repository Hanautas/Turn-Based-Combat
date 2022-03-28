using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatEncounter : MonoBehaviour
{
    public List<Encounter> encounters;

    void Reset()
    {
        encounters = new List<Encounter>()
        {
            new Encounter()
        };
    }

    public void StartCombat()
    {
        //GameManager.instance.LoadScene("Combat");
    }
}

[System.Serializable]
public class Encounter
{
    public Sprite enemySprite;
    public UnitData enemyUnitData;
}