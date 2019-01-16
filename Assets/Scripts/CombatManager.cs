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
    Character playerChar;
    Character enemyChar;
    SkillScript playerSkillScript;
    SkillScript enemySkillScript;
    BattleBGMScript myBattleBGMScript;
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

        playerSkillScript = playerObjectScript.GetPlayerSkillScript();
        enemySkillScript = enemyObjectScript.GetEnemySkillScript();

        myCombatUIManager = FindObjectOfType<CombatUIManager>();
        myBattleBGMScript = FindObjectOfType<BattleBGMScript>();

        myBattleBGMScript.BeginBattleBGM();
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

        // Bind the command chosen and start to initiate this round's combat 
        enemyCommand = enemyObjectScript.GetChosenEnemyCommand();
        playerCommand = playerObjectScript.GetPlayerChosenCommand();
        myCombatUIManager.ThisTurnCombatCommandResult(playerCommand, enemyCommand);

        myCombatUIManager.PreCombatUIProcedure();

        // Activate all Pre-Combat abilities 
        PreCombatEndCombatSkillActivation(playerChar, 0, ActivationTime.beforeCombat);
    }

    // Use Dynamic Programming to process skill count
    private void PreCombatEndCombatSkillActivation(Character currentChar, int processCount, ActivationTime currentTime)
    {
        bool skillActivated = false;

        // Once done process both character, move on to next phase
        if (processCount == 2)
        {
            StartCoroutine(ProgressNextPhaseAfterSkillResolved(currentTime));
            return;
        }

        processCount++;

        if (currentChar == playerChar)
        {
            playerObjectScript.PlayerAttemptToUseSkill(currentTime);
            skillActivated = playerSkillScript.GetIsThisCharacterUsingSkill();

            if (skillActivated) { StartCoroutine(PreCombatWaitForKeyPress(playerSkillScript, processCount, enemyChar, currentTime)); }
            else { PreCombatEndCombatSkillActivation(enemyChar, processCount, currentTime); }
        }
        else if (currentChar == enemyChar)
        {
            enemyObjectScript.EnemyAttemptToUseSkill(currentTime);
            skillActivated = enemySkillScript.GetIsThisCharacterUsingSkill();

            if (skillActivated) { StartCoroutine(PreCombatWaitForKeyPress(enemySkillScript, processCount, playerChar, currentTime)); }
            else { PreCombatEndCombatSkillActivation(playerChar, processCount, currentTime); }
        }
    }

    IEnumerator ProgressNextPhaseAfterSkillResolved(ActivationTime currentTime)
    {
        myCombatUIManager.SetTurnOutcomeText("All effects in this phase are resolved.\nTap to progress the battle.");

        // After combat, resolving buff debuff duration
        if (currentTime == ActivationTime.afterCombat)
        {
            playerChar.CheckForThisCharacterBuffDebuffDuration();
            enemyChar.CheckForThisCharacterBuffDebuffDuration();
        }

        yield return WaitForKeyPress();

        if (currentTime == ActivationTime.beforeCombat)
        {
            StartCoroutine(BeginCombatProcedure());
        }
        else if (currentTime == ActivationTime.afterCombat)
        {
            NextCombatTurnProcedure();
        }
    }

    IEnumerator PreCombatWaitForKeyPress(SkillScript charSkillScript, int processCount, Character nextCharacter, ActivationTime currentTime)
    {
        yield return WaitForKeyPress();
        charSkillScript.SetThisCharacterUsingSkill(false);
        PreCombatEndCombatSkillActivation(nextCharacter, processCount, currentTime);
    }

    // Begin the CORE function of the combat
    IEnumerator BeginCombatProcedure()
    {
        //yield return WaitForKeyPress();

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
    }

    private void FastestUnitCombatProcedure(int playerSpeed, int enemySpeed)
    {
        if (playerSpeed >= enemySpeed)
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
        }
        
        switch (playerCommand)
        {
            case CombatCommand.attack:
                if (isPlayerTurn)
                {
                    PlayerAttackingEnemy(playerChar, enemyChar, playerStrength);
                }
                else
                {
                    if (enemyCommand == CombatCommand.attack)
                    {
                        EnemyAttackingPlayer(playerChar, enemyChar, enemyStrength);
                    }
                    else if (enemyCommand == CombatCommand.powerAttack)
                    {
                        EnemyAttackingPlayer(playerChar, enemyChar, enemyStrength*2);
                    }
                    else if (enemyCommand == CombatCommand.guard)
                    {
                        EnemyIsGuarding();
                    }
                }
                break;

            case CombatCommand.powerAttack:
                if (enemyCommand == CombatCommand.attack)
                {
                    if (isPlayerTurn)
                    {
                        PlayerAttackingEnemy(playerChar, enemyChar, playerStrength * 2);
                    }
                    else
                    {
                        EnemyAttackingPlayer(playerChar, enemyChar, enemyStrength);
                    }
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    if (isPlayerTurn)
                    {
                        PlayerAttackingEnemy(playerChar, enemyChar, playerStrength * 2);
                    }
                    else
                    {
                        EnemyAttackingPlayer(playerChar, enemyChar, enemyStrength * 2);
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
                        //myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                        EnemyIsGuarding();
                    }
                }
                break;

            case CombatCommand.guard:
                if (isPlayerTurn)
                {
                    //myCombatUIManager.DisplayDuringCombatText(turnUnit, playerCommand, enemyCommand);
                    PlayerIsGuarding();
                }
                else
                {
                    if (enemyCommand == CombatCommand.attack)
                    {
                        EnemyAttackingPlayer(playerChar, enemyChar, enemyStrength);
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
                        // myCombatUIManager.DisplayDuringCombatText(turnUnit, enemyCommand, playerCommand);
                        EnemyIsGuarding();
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
        attackingChar.PlayAttackSFX();
        targetChar.ThisCharacterTakingDamage(damage);
        myCombatUIManager.DisplayDuringCombatText(attackingChar.gameObject, attackingCommand, targetCommand);
    }

    private void PlayerAttackingEnemy(Character playerChar, Character enemyChar, int damage)
    {
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.duringCombatOffense);

        bool playerUseSkill = playerSkillScript.GetIsThisCharacterUsingSkill();

        // Normal attack if no skill is used
        if (!playerUseSkill) { StartCoroutine(BattleOutcome(playerChar, playerCommand, enemyChar, enemyCommand, damage)); }

        // TODO add enemy defense skill

        // Reset the boolean back to false
        playerSkillScript.SetThisCharacterUsingSkill(false);
    }

    private void EnemyAttackingPlayer(Character playerChar, Character enemyChar, int damage)
    {
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.duringCombatOffense);

        bool enemyUseSkill = enemySkillScript.GetIsThisCharacterUsingSkill();

        // Normal attack if no skill is used
        if (!enemyUseSkill) { StartCoroutine(BattleOutcome(enemyChar, enemyCommand, playerChar, playerCommand, damage)); }

        // TODO add player defense skill

        // Reset the boolean back to false
        enemySkillScript.SetThisCharacterUsingSkill(false);
    }

    private void PlayerIsGuarding()
    {
        playerObjectScript.PlayerAttemptToUseSkill(ActivationTime.duringCombatOffense);

        bool playerUseSkill = playerSkillScript.GetIsThisCharacterUsingSkill();

        if (!playerUseSkill) { myCombatUIManager.DisplayDuringCombatText(playerChar.gameObject, playerCommand, enemyCommand); }

        playerSkillScript.SetThisCharacterUsingSkill(false);
    }

    private void EnemyIsGuarding()
    {
        enemyObjectScript.EnemyAttemptToUseSkill(ActivationTime.duringCombatOffense);

        bool enemyUseSkill = enemySkillScript.GetIsThisCharacterUsingSkill();

        if (!enemyUseSkill) { myCombatUIManager.DisplayDuringCombatText(enemyChar.gameObject, enemyCommand, playerCommand); }

        enemySkillScript.SetThisCharacterUsingSkill(false);
    }


    private void EndOfCombatProcedure()
    {
        // Activate End of Combat Skills
        PreCombatEndCombatSkillActivation(playerChar, 0, ActivationTime.afterCombat);
        myCombatUIManager.EndOfCombatTurnUI();
    }

    private void NextCombatTurnProcedure()
    {
        myCombatUIManager.NextCombatTurnUI();
    }

    public void EndOfCombat(GameObject defeatedCharacter)
    {
        combatEnded = true;

        if (defeatedCharacter == enemyChar.gameObject)
        {
            myBattleBGMScript.BeginVictoryBGM();
        }
        else
        {
            myBattleBGMScript.BeginLosingBGM();
        }

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
