using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    CombatCommand chosenCommand;
    Character myCharacter;
    Character myOpponent;
    CombatManager myCombatManager;
    SkillScript mySkillScript;

    // Use Awake as CombatManagers need to take in character in Start
    void Awake()
    {
        myCharacter = GetComponent<Character>();
        myOpponent = FindObjectOfType<EnemyScript>().GetComponent<Character>();
        myCombatManager = FindObjectOfType<CombatManager>();
        mySkillScript = GetComponent<SkillScript>();

        mySkillScript.SetThisCharacterOpponent(myOpponent);
    }

    public void OnClickExecuteCommand(int commandInt)
    {
        chosenCommand = (CombatCommand)commandInt;      // Cast Int to Enum
        myCombatManager.StartThisTurnProgression();
    }


    // Getter and Setter
    public CombatCommand GetPlayerChosenCommand()
    {
        return chosenCommand;
    }

    public Character GetPlayerCharacter()
    {
        return myCharacter;
    }

    public SkillScript GetPlayerSkillScript()
    {
        return mySkillScript;
    }
}
