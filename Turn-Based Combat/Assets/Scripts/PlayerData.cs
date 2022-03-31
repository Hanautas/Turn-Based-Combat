using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public PlayableCharacter[] playableCharacters;

    void Awake()
    {
        instance = this;
    }

    public void RecruitCharacter(int index)
    {
        playableCharacters[index].isRecruited = true;
    }

    public void RemoveCharacter(int index)
    {
        playableCharacters[index].isRecruited = false;
    }

    public bool GetIsRecruited(int index)
    {
        if (playableCharacters[index].isRecruited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public UnitData GetUnitData(int index)
    {
        return playableCharacters[index].unitData;
    }

    public Sprite[] GetUnitSprites(int index)
    {
        return playableCharacters[index].unitSprites;
    }
}