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

    public virtual void Activate()
    {
        Debug.Log("No Function!");
    }
}