using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public int cost;
    public string name;
    [TextArea(3, 5)]
    public string description;
}