using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatCommand
{
    attack = 1,
    powerAttack = 2,
    guard = 3
}

public class CombatManager : MonoBehaviour {

    PlayerScript playerObjectScript;
    EnemyScript enemyObjectScript;
    CombatUIManager myCombatUIManager;

    CombatCommand playerCommand;
    CombatCommand enemyCommand;

	// Use this for initialization
	void Start ()
    {
        playerObjectScript = FindObjectOfType<PlayerScript>();
        enemyObjectScript = FindObjectOfType<EnemyScript>();
        myCombatUIManager = FindObjectOfType<CombatUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ProgressCurrentTurn()
    {
        myCombatUIManager.ManageCommandButton(false);
        enemyObjectScript.PickingEnemyCommand();
        StartCoroutine(ProcessOutcomeDelay());
    }

    IEnumerator ProcessOutcomeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        enemyCommand = enemyObjectScript.GetChosenEnemyCommand();
        playerCommand = playerObjectScript.GetPlayerChosenCommand();
        ProcessCombatOutcome();
        myCombatUIManager.EndOfTurnProcess();
    }

    // Processing Combat
    // TODO check damage sequence, there will be no scenario where both player and enemy die together!
    void ProcessCombatOutcome()
    {
        int playerStrength = playerObjectScript.GetPlayerStrength();
        int enemyStrength = enemyObjectScript.GetEnemyStrength();

        switch (playerCommand)
        {
            case CombatCommand.attack:
                if (enemyCommand == CombatCommand.attack)
                {
                    enemyObjectScript.EnemyResolveDamage(playerStrength);
                    playerObjectScript.PlayerResolveDamage(enemyStrength);
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    enemyObjectScript.EnemyResolveDamage(playerStrength);
                    playerObjectScript.PlayerResolveDamage(enemyStrength * 2);
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    enemyObjectScript.EnemyResolveDamage(playerStrength);
                }
                break;

            case CombatCommand.powerAttack:
                if (enemyCommand == CombatCommand.attack)
                {
                    enemyObjectScript.EnemyResolveDamage(playerStrength * 2);
                    playerObjectScript.PlayerResolveDamage(enemyStrength);
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    enemyObjectScript.EnemyResolveDamage(playerStrength * 2);
                    playerObjectScript.PlayerResolveDamage(enemyStrength * 2);
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    // Enemy evaded player's P.Attack
                }
                break;

            case CombatCommand.guard:
                if (enemyCommand == CombatCommand.attack)
                {
                    playerObjectScript.PlayerResolveDamage(enemyStrength);
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    // Player evaded enemy's P.Attack
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    // Both guarding
                }
                break;

            default:
                Debug.LogError("Error processing combat outcome.");
                break;
        }

        myCombatUIManager.ThisTurnCombatOutcome(playerCommand, enemyCommand);
    }

    public void EndOfCombat(GameObject defeatedCharacter)
    {
        myCombatUIManager.EndOfCombatResult(defeatedCharacter);
    }
}
