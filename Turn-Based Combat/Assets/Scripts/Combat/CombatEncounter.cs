using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter : MonoBehaviour
{
    public List<UnitData> enemyUnits;

    void Reset()
    {
        enemyUnits = new List<UnitData>()
        {
            new UnitData()
        };
    }

    public void StartCombat()
    {
        //GameManager.instance.LoadScene("Combat");
    }
}