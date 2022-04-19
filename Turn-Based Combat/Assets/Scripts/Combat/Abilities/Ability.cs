using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public int cost;

    public Sprite icon;

    public string abilityName;
    [TextArea(3, 5)]
    public string abilityDescription;

    public virtual void SetMode()
    {
        Debug.Log("No Function!");
    }

    public virtual void Activate(Unit target)
    {
        Debug.Log("No Function!");
    }
}