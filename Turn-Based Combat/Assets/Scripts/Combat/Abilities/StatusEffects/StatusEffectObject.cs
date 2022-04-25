using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectObject
{
    public int duration;

    public StatusEffect statusEffect;

    public StatusEffectObject(StatusEffect newStatusEffect)
    {
        statusEffect = newStatusEffect;

        duration = newStatusEffect.duration;
    }

    public void Activate(Unit target)
    {
        statusEffect.Activate(target);

        SetDuration(-1);
    }

    public void SetDuration(int amount)
    {
        duration += amount;
    }
}