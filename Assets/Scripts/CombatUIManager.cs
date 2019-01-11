using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour {
    [Header("General Combat UI")]
    [SerializeField] Text turnCountText;
    [SerializeField] Text turnOutcome;

    [Header("Player Combat UI")]
    [SerializeField] Text playerCommandText;
    [SerializeField] Animator playerCommandTextBoxAnimator;
    [SerializeField] Text playerSkillText;
    [SerializeField] Animator playerSkillTextBoxAnimator;

    [Header("Enemy Combat UI")]
    [SerializeField] Text enemyCommandText;
    [SerializeField] Animator enemyCommandTextBoxAnimator;
    [SerializeField] Text enemySkillText;
    [SerializeField] Animator enemySkillTextBoxAnimator;

    [Header("End of Combat UI")]
    [SerializeField] GameObject endofCombatPanel;
    [SerializeField] Text combatOutcome;

    [Header("Buff Debuff Duration UI")]
    [SerializeField] List<Text> playerBuffDuration;
    [SerializeField] List<Text> playerDebuffDuration;
    [SerializeField] List<Text> enemyBuffDuration;
    [SerializeField] List<Text> enemyDebuffDuration;

    [Header("Other Settings")]
    [SerializeField] GameObject processingTurnPanel;
    [SerializeField] Text currentPhaseText;
    [SerializeField] Text currentPhaseSubText;

    Character playerChar;
    Character enemyChar;

    int turnCount = 0;


    // START : Start of Combat (Initialization & Beginning of Combat)
    void Start ()
    {
        playerChar = FindObjectOfType<PlayerScript>().GetPlayerCharacter();
        enemyChar = FindObjectOfType<EnemyScript>().GetEnemyCharacter();
    }

    public void CombatIntroductionStart()
    {
        processingTurnPanel.SetActive(true);
        currentPhaseText.text = "Starting Battle...";
        currentPhaseSubText.text = "Encountered " + enemyChar.GetComponent<EnemyScript>().GetEnemyName() + "!\nTap screen to begin";
    }

    public void CombatIntroductionEnd()
    {
        NextCombatTurnUI();
        currentPhaseSubText.text = "Progressing current turn.\nTap screen to continue.";
    }
    // END : Start of Combat



    // START : Pre-Combat (Display of command result)
    // Combat Command text change based on player and enemy commands
    public void ThisTurnCombatCommandResult(CombatCommand playerCommand, CombatCommand enemyCommand)
    {
        string playerCommandString = playerCommand.ToString();
        string enemyCommandString = enemyCommand.ToString();

        if (playerCommand == CombatCommand.powerAttack) { playerCommandString = "Power attack"; }
        if (enemyCommand == CombatCommand.powerAttack) { enemyCommandString = "Power attack"; }

        // Change the first letter to upper case followed by the rest of the string (substring)
        playerCommandString = char.ToUpper(playerCommandString[0]) + playerCommandString.Substring(1);
        enemyCommandString = char.ToUpper(enemyCommandString[0]) + enemyCommandString.Substring(1);

        playerCommandText.text = playerCommandString;
        enemyCommandText.text = enemyCommandString;
    }

    // Phase text change
    public void PreCombatUIProcedure()
    {
        currentPhaseText.text = "Pre-Combat Phase";
        CommandTextBoxDisplay(true);
        processingTurnPanel.SetActive(true);
    }

    // Display of Combat Command
    public void CommandTextBoxDisplay(bool status)
    {
        playerCommandTextBoxAnimator.SetBool("AppearUIWindow", status);
        enemyCommandTextBoxAnimator.SetBool("AppearUIWindow", status);
    }
    // END : Pre-Combat


    // TODO consider changing tag -> checking playerChar and enemyChar GO
    // START : During Combat 
    // Displaying combat outcome based on turn
    public void DisplayDuringCombatText(GameObject unitTurn, CombatCommand turnUnitCommand, CombatCommand targetCommand)
    {
        int targetSufferedDamage = -1;
        string thisUnitTurnName = "Dummy";
        string targetName = "Dummy2";

        if (unitTurn.tag == "Player")
        {
            thisUnitTurnName = "Player";
            targetName = "Enemy";
            targetSufferedDamage = enemyChar.GetThisTurnCharDamage();
        }
        else if (unitTurn.tag == "Enemy")
        {
            thisUnitTurnName = "Enemy";
            targetName = "Player";
            targetSufferedDamage = playerChar.GetThisTurnCharDamage();
        }

        switch (turnUnitCommand)
        {
            case CombatCommand.attack:
                turnOutcome.text = thisUnitTurnName + " attacks!\nDealing " + targetSufferedDamage.ToString() + " to " + targetName + "!";
                break;

            case CombatCommand.powerAttack:
                if (targetCommand == CombatCommand.attack || targetCommand == CombatCommand.powerAttack)
                {
                    turnOutcome.text = thisUnitTurnName + " uses power attack!\nDealing " + targetSufferedDamage.ToString() + " to " + targetName + "!";
                }
                else if (targetCommand == CombatCommand.guard)
                {
                    turnOutcome.text = thisUnitTurnName + " uses power attack!\nHowever, the " + targetName + " guarded the attack. Dealing no damage!";
                }
                break;

            case CombatCommand.guard:
                turnOutcome.text = thisUnitTurnName + " uses guard!";
                break;

            default:
                Debug.LogError("Error processing combat outcome.");
                break;
            }

    }
    // END : During Combat



    // START : End of current turn & preparing for next turn
    public void EndOfCombatTurnUI()
    {
        currentPhaseText.text = "Ending Phase";
    }

    public void NextCombatTurnUI()
    {
        processingTurnPanel.SetActive(false);
        turnCount++;
        turnCountText.text = "TURN " + turnCount;
        turnOutcome.text = "Choose a command.";
    }
    // END : End of current turn & preparing for next turn



    // START : General - All Timing
    // Display Skill display regardless timing
    public void UsingSkillDisplayUI(GameObject currentGO, string skillName)
    {
        if (currentGO.tag == "Player")
        {
            playerSkillText.text = "Activating " + skillName;
            playerSkillTextBoxAnimator.SetBool("SlideInWindow", true);
        }
        else if (currentGO.tag == "Enemy")
        {
            enemySkillText.text = "Activating " + skillName;
            enemySkillTextBoxAnimator.SetBool("SlideInWindow", true);
        }
        else
        {
            Debug.Log("Please assign the gameobject who use the skill a proper TAG");
        }
    }

    public void TurnOffSkillUI()
    {
        playerSkillTextBoxAnimator.SetBool("SlideInWindow", false);
        enemySkillTextBoxAnimator.SetBool("SlideInWindow", false);
    }

    public void SetTurnOutcomeText(string currentString)
    {
        turnOutcome.text = currentString;
    }
    // END : General - All Timing



    // End of the whole combat
    public void EndOfCombatResult(GameObject defeatedCharacter)
    {
        endofCombatPanel.SetActive(true);

        // TODO Please address the issue of draw. Player should win the moment the last attack lands!
        if (defeatedCharacter == playerChar.gameObject)
        {
            combatOutcome.text = "Player has been defeated!\nYou lost!";
        }
        else
        {
            combatOutcome.text = "Enemy has been defeated!\nYou win!";
        }
    }

    // Setter
    public void SetCurrentPhaseText(string currentText)
    {
        currentPhaseText.text = currentText;
    }

    public void SetCharacterStatDurationText(Character character, List<int> buffDurationLeft, List<int> debuffDurationLeft)
    {
        if (character == playerChar)
        {
            for (int i = 0; i < playerBuffDuration.Count; i++)
            {
                if (buffDurationLeft[i] == 0)
                {
                    playerBuffDuration[i].text = "-";
                }
                else
                {
                    playerBuffDuration[i].text = buffDurationLeft[i].ToString();
                }
            }

            for (int j = 0; j < playerDebuffDuration.Count; j++)
            {
                if (debuffDurationLeft[j] == 0)
                {
                    playerDebuffDuration[j].text = "-";
                }
                else
                {
                    playerDebuffDuration[j].text = debuffDurationLeft[j].ToString();
                }
            }
        }
        else if (character == enemyChar)
        {
            for (int k = 0; k < enemyBuffDuration.Count; k++)
            {
                if (buffDurationLeft[k] == 0)
                {
                    enemyBuffDuration[k].text = "-";
                }
                else
                {
                    enemyBuffDuration[k].text = buffDurationLeft[k].ToString();
                }
            }

            for (int l = 0; l < enemyDebuffDuration.Count; l++)
            {
                if (debuffDurationLeft[l] == 0)
                {
                    enemyDebuffDuration[l].text = "-";
                }
                else
                {
                    enemyDebuffDuration[l].text = debuffDurationLeft[l].ToString();
                }
            }
        }
    }
}
