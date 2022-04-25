using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string statusEffectName;

    public int duration;

    public virtual void Activate(Unit target)
    {
        Debug.Log("No Function!");
    }
}