using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Info")]
    [SerializeField] EnemyInfo enemyEncountered;

    CombatCommand chosenEnemyCommand;
    Character myCharacter;
    SkillScript mySkillScript;

    // Use Awake as CombatManagers need to take in character in Start
    void Awake ()
    {
        myCharacter = GetComponent<Character>();
        mySkillScript = GetComponent<SkillScript>();

        GetComponent<SpriteRenderer>().sprite = enemyEncountered.GetEnemySprite();
    }
    
    public void PickingEnemyCommand()
    {
        int generateNumber = Random.Range(1, 300);

        if (generateNumber % 3 == 0)
        {
            chosenEnemyCommand = CombatCommand.attack;
        }
        else if (generateNumber % 3 == 1)
        {
            chosenEnemyCommand = CombatCommand.powerAttack;
        }
        else
        {
            chosenEnemyCommand = CombatCommand.guard;
        }
    }


    // Public Functions
    public void EnemyAttemptToUseSkill(ActivationTime currentTime)
    {
        mySkillScript.AttemptToUseSkill(currentTime, chosenEnemyCommand);
    }


    // Getter and Setter
    public CombatCommand GetChosenEnemyCommand()
    {
        return chosenEnemyCommand;
    }

    public EnemyInfo GetThisEnemyInfo()
    {
        return enemyEncountered;
    }

    public string GetEnemyName()
    {
        return enemyEncountered.GetEnemyName();
    }

    public Character GetEnemyCharacter()
    {
        return myCharacter;
    }
}
