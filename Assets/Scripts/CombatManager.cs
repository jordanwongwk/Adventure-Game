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

    // Activated when player chose his/her command
    public void StartThisTurnProgression()
    {
        // RNG a command for enemy
        enemyObjectScript.PickingEnemyCommand();

        // Activate all Pre-Combat abilities
        // TODO add enemy skill
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.beforeCombat);
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.beforeCombat);

        // Bind the command chosen and start to initiate this round's combat 
        enemyCommand = enemyObjectScript.GetChosenEnemyCommand();
        playerCommand = playerObjectScript.GetPlayerChosenCommand();
        myCombatUIManager.ThisTurnCombatOutcome(playerCommand, enemyCommand);       // End of turn process is done after coroutine here       
    }

    // Processing During Combat Outcome (turnUnit = unit that has a higher speed are selected first)
    public void ProcessingDuringCombatPhase(GameObject turnUnit)
    {
        Character playerChar = playerObjectScript.GetPlayerCharacter();
        Character enemyChar = enemyObjectScript.GetEnemyCharacter();

        int playerStrength = playerChar.GetThisCharStrength();
        int enemyStrength = enemyChar.GetThisCharStrength();
        bool isPlayerTurn = false;

        // if its player's turn
        if (turnUnit == playerObjectScript.gameObject)
        {
            isPlayerTurn = true;
            playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.duringCombat);
        }
        else
        {
            enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.duringCombat);
            // enemy attempt skill
            Debug.Log("Enemy SKILL TIME");
        }

        switch (playerCommand)
        {
            case CombatCommand.attack:
                if (isPlayerTurn)
                {
                    StartCoroutine(BattleOutcome(playerChar, playerCommand, enemyChar, enemyCommand, playerStrength));
                }
                else
                {
                    if (enemyCommand == CombatCommand.attack)
                    {
                        StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, enemyStrength));
                    }
                    else if (enemyCommand == CombatCommand.powerAttack)
                    {
                        StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, enemyStrength * 2));
                    }
                    else if (enemyCommand == CombatCommand.guard)
                    {
                        myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                    }
                }
                break;

            case CombatCommand.powerAttack:
                if (enemyCommand == CombatCommand.attack)
                {
                    if (isPlayerTurn)
                    {
                        StartCoroutine(BattleOutcome(playerChar, playerCommand, enemyChar, enemyCommand, playerStrength * 2));
                    }
                    else
                    {
                        StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, enemyStrength));
                    }
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    if (isPlayerTurn)
                    {
                        StartCoroutine(BattleOutcome(playerChar, playerCommand, enemyChar, enemyCommand, playerStrength * 2));
                    }
                    else
                    {
                        StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, enemyStrength * 2));
                    }
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    // Enemy evaded player's P.Attack
                    if (isPlayerTurn)
                    {
                        playerChar.AttackAnimation();
                        enemyChar.GuardPowerAttackAnimation();
                        myCombatUIManager.DisplayDuringCombatText(turnUnit, playerCommand, enemyCommand);
                    }
                    else
                    {
                        myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                    }
                }
                break;

            case CombatCommand.guard:
                if (isPlayerTurn)
                {
                    myCombatUIManager.DisplayDuringCombatText(turnUnit, playerCommand, enemyCommand);
                }
                else
                {
                    if (enemyCommand == CombatCommand.attack)
                    {
                        StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, enemyStrength));
                    }
                    else if (enemyCommand == CombatCommand.powerAttack)
                    {
                        // Player evaded enemy's P.Attack
                        enemyChar.AttackAnimation();
                        playerChar.GuardPowerAttackAnimation();
                        myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                    }
                    else if (enemyCommand == CombatCommand.guard)
                    {
                        // Both guarding
                        myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                    }
                }
                
                break;

            default:
                Debug.LogError("Error processing combat outcome.");
                break;
        }
    }

    IEnumerator BattleOutcome(Character attackingChar, CombatCommand attackingCommand, Character targetChar, CombatCommand targetCommand, int damage)
    {
        attackingChar.AttackAnimation();
        yield return new WaitForSeconds(0.3f);      //TODO find length
        targetChar.ThisCharacterTakingDamage(damage);
        myCombatUIManager.DisplayDuringCombatText(attackingChar.gameObject, attackingCommand, targetCommand);
    }


    public void EndOfTurnProcess()
    {
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.afterCombat);
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.afterCombat);
    }

    public void EndOfCombat(GameObject defeatedCharacter)
    {
        myCombatUIManager.SetCombatHasEnded(true);
        myCombatUIManager.EndOfCombatResult(defeatedCharacter);
    }
}
