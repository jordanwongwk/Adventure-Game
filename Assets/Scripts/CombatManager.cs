﻿using System.Collections;
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
    Character playerChar;
    Character enemyChar;
    CombatUIManager myCombatUIManager;

    CombatCommand playerCommand;
    CombatCommand enemyCommand;

    bool combatEnded = false;

	// Initialization and Start of Combat
	void Start ()
    {
        playerObjectScript = FindObjectOfType<PlayerScript>();
        enemyObjectScript = FindObjectOfType<EnemyScript>();

        playerChar = playerObjectScript.GetPlayerCharacter();
        enemyChar = enemyObjectScript.GetEnemyCharacter();

        myCombatUIManager = FindObjectOfType<CombatUIManager>();

        StartCoroutine(StartCombatProcedure());
    }

    IEnumerator StartCombatProcedure()
    {
        myCombatUIManager.CombatIntroductionStart();
        yield return WaitForKeyPress();
        myCombatUIManager.CombatIntroductionEnd();
    }


    // Called from playerScript when player chose his/her command
    public void StartThisTurnProgression()
    {
        // RNG a command for enemy
        enemyObjectScript.PickingEnemyCommand();

        // Activate all Pre-Combat abilities
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.beforeCombat);
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.beforeCombat);

        // Bind the command chosen and start to initiate this round's combat 
        enemyCommand = enemyObjectScript.GetChosenEnemyCommand();
        playerCommand = playerObjectScript.GetPlayerChosenCommand();
        myCombatUIManager.ThisTurnCombatCommandResult(playerCommand, enemyCommand);       // End of turn process is done after coroutine here       
        StartCoroutine(BeginCombatProcedure());
    }

    // Begin the CORE function of the combat
    IEnumerator BeginCombatProcedure()
    {
        myCombatUIManager.PreCombatUIProcedure();

        yield return WaitForKeyPress();

        myCombatUIManager.CommandTextBoxDisplay(false);

        yield return new WaitForSeconds(0.5f);      // Delay for window to disappear OR animation to finish

        int playerSpeed = playerChar.GetThisCharSpeed();
        int enemySpeed = enemyChar.GetThisCharSpeed();

        FastestUnitCombatProcedure(playerSpeed, enemySpeed);
        // TODO activate defense skill on slowest speed unit

        yield return WaitForKeyPress();

        SlowestUnitCombatProcedure(playerSpeed, enemySpeed);
        // TODO activate defense skill on fastest speed unit

        yield return WaitForKeyPress();

        EndOfCombatProcedure();

        yield return WaitForKeyPress();

        NextCombatTurnProcedure();
    }

    private void FastestUnitCombatProcedure(int playerSpeed, int enemySpeed)
    {
        if (playerSpeed >= enemySpeed)
        {
            ProcessingDuringCombatPhase(playerChar.gameObject);
            myCombatUIManager.SetCurrentPhaseText("Player's Turn");

            // if (skill == true)
            //yield return WaitForKeyPress();
            //Debug.Log("Allololol");
        }
        else
        {
            ProcessingDuringCombatPhase(enemyChar.gameObject);
            myCombatUIManager.SetCurrentPhaseText("Enemy's Turn");
        }
    }

    public void SlowestUnitCombatProcedure(int playerSpeed, int enemySpeed)
    {
        if (playerSpeed < enemySpeed)
        {
            ProcessingDuringCombatPhase(playerChar.gameObject);
            myCombatUIManager.SetCurrentPhaseText("Player's Turn");
        }
        else
        {
            ProcessingDuringCombatPhase(enemyChar.gameObject);
            myCombatUIManager.SetCurrentPhaseText("Enemy's Turn");
        }
    }

    // Processing During Combat Outcome (turnUnit = unit that has a higher speed are selected first)
    private void ProcessingDuringCombatPhase(GameObject turnUnit)
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
            playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.duringCombatOffense);
        }
        else
        {
            enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.duringCombatOffense);
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


    private void EndOfCombatProcedure()
    {
        // Activate End of Combat Skills
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.afterCombat);
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.afterCombat);

        myCombatUIManager.EndOfCombatTurnUI();
    }

    private void NextCombatTurnProcedure()
    {
        myCombatUIManager.NextCombatTurnUI();
    }

    public void EndOfCombat(GameObject defeatedCharacter)
    {
        combatEnded = true;
        myCombatUIManager.EndOfCombatResult(defeatedCharacter);
    }



    IEnumerator WaitForKeyPress()
    {
        bool isPressed = false;
        while (!isPressed)
        {
            if (Input.GetMouseButtonDown(0) && !combatEnded)
            {
                isPressed = true;

                // Turn off Skill UI upon key press
                myCombatUIManager.TurnOffSkillUI();
            }
            yield return null;
        }
    }
}
