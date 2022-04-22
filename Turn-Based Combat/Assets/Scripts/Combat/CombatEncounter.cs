using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatEncounter : MonoBehaviour
{
    public List<Entity> entities;

    void Reset()
    {
        entities = new List<Entity>()
        {
            new Entity()
        };
    }

    public void StartCombat()
    {
        //GameManager.instance.LoadScene("Combat");
    }
}