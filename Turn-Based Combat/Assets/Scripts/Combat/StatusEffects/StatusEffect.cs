using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public string statusEffectName;

    public virtual void Activate(Unit target)
    {
        Debug.Log("No Function!");
    }
}