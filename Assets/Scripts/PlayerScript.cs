using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    CombatCommand chosenCommand;
    Character myCharacter;
    CombatManager myCombatManager;
    SkillScript mySkillScript;

    // Use Awake as CombatManagers need to take in character in Start
    void Awake()
    {
        myCharacter = GetComponent<Character>();
        myCombatManager = FindObjectOfType<CombatManager>();
        mySkillScript = GetComponent<SkillScript>();
    }

    public void OnClickExecuteCommand(int commandInt)
    {
        chosenCommand = (CombatCommand)commandInt;      // Cast Int to Enum
        myCombatManager.StartThisTurnProgression();
    }

    // Public Command
    public void PlayerAttemptToUseSkill(ActivationTime currentTime)
    {
        mySkillScript.AttemptToUseSkill(currentTime, chosenCommand);
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
}
