using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string MONSTER_TO_BATTLE = "monster_to_battle";


    public void SetMonsterToBattle(int monsterID)
    {
        PlayerPrefs.SetInt(MONSTER_TO_BATTLE, monsterID);
    }

    public int GetMonsterToBattle()
    {
        return PlayerPrefs.GetInt(MONSTER_TO_BATTLE);
    }
}
