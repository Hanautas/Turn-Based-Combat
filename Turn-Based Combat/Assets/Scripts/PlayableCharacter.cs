using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    None,
    TheRemnants,
    REDACTED
}

[System.Serializable]
public class PlayableCharacter
{
    public bool isRecruited;

    [Header("Character")]
    public string characterName;
    public int age;
    public float height;
    public string likes;
    public string dislikes;

    public Faction faction;

    [Header("Combat")]
    public UnitData unitData;
    public Sprite[] unitSprites;
}