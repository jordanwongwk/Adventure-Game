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

    [Header("Other Settings")]
    [SerializeField] GameObject processingTurnPanel;
    [SerializeField] Text currentPhaseText;
    [SerializeField] Text currentPhaseSubText;

    Character playerChar;
    Character enemyChar;
    CombatManager myCombatManager;

    int turnCount = 0;
    bool combatEnded = false;

    // Use this for initialization
    void Start ()
    {
        playerChar = FindObjectOfType<PlayerScript>().GetPlayerCharacter();
        enemyChar = FindObjectOfType<EnemyScript>().GetEnemyCharacter();
        myCombatManager = FindObjectOfType<CombatManager>();

        StartCoroutine(StartCombatCoroutine());
    }

    IEnumerator StartCombatCoroutine()
    {
        processingTurnPanel.SetActive(true);
        currentPhaseText.text = "Starting Battle...";
        currentPhaseSubText.text = "Encountered " + enemyChar.GetComponent<EnemyScript>().GetEnemyName() + "!\nTap screen to begin";

        yield return WaitForKeyPress();

        ProceedToNewTurn();
        currentPhaseSubText.text = "Progressing current turn.\nTap screen to continue.";
    }

    // Initiating combat (After pressing commands)
    public void ThisTurnCombatOutcome(CombatCommand playerCommand, CombatCommand enemyCommand)
    {
        string playerCommandString = playerCommand.ToString();
        string enemyCommandString = enemyCommand.ToString();

        if (playerCommand == CombatCommand.powerAttack) { playerCommandString = "power attack"; }
        if (enemyCommand == CombatCommand.powerAttack) { enemyCommandString = "power attack"; }

        playerCommandText.text = "Player uses " + playerCommandString + "!";
        enemyCommandText.text = "Enemy uses " + enemyCommandString + "!";
        StartCoroutine(InitiatingThisTurn(playerCommand, enemyCommand));
    }

    // Start the turn
    IEnumerator InitiatingThisTurn(CombatCommand playerCommand, CombatCommand enemyCommand)
    {
        // PHASE ONE: PRE-COMBAT
        currentPhaseText.text = "Pre-Combat Phase";
        playerCommandTextBoxAnimator.SetBool("AppearUIWindow", true);
        enemyCommandTextBoxAnimator.SetBool("AppearUIWindow", true);
        processingTurnPanel.SetActive(true);

        // Pre-Combat here

        yield return WaitForKeyPress();

        // PHASE TWO: DURING COMBAT


        playerCommandTextBoxAnimator.SetBool("AppearUIWindow", false);
        enemyCommandTextBoxAnimator.SetBool("AppearUIWindow", false);

        yield return new WaitForSeconds(0.5f);      // Delay for window to disappear OR animation to finish

        // PHASE 2-1: FASTEST unit actions.
        int playerSpeed = playerChar.GetThisCharSpeed();
        int enemySpeed = enemyChar.GetThisCharSpeed();

        // Consider refactoring the following and add animation, if want
        if (playerSpeed >= enemySpeed)
        {
            myCombatManager.ProcessingDuringCombatPhase(playerChar.gameObject);
            currentPhaseText.text = "Player's Turn";

            // if (skill == true)
               //yield return WaitForKeyPress();
               //Debug.Log("Allololol");
        }
        else
        {
            myCombatManager.ProcessingDuringCombatPhase(enemyChar.gameObject);
            currentPhaseText.text = "Enemy's Turn";
        }

        yield return WaitForKeyPress();
        Debug.Log("Next");
        // PHASE 2-2: SLOWEST unit actions.
        if (playerSpeed < enemySpeed)
        {
            myCombatManager.ProcessingDuringCombatPhase(playerChar.gameObject);
            currentPhaseText.text = "Player's Turn";
        }
        else
        {
            myCombatManager.ProcessingDuringCombatPhase(enemyChar.gameObject);
            currentPhaseText.text = "Enemy's Turn";
        }

        

        yield return WaitForKeyPress();

        // PHASE 3: END OF TURN
        InitiatingEndofTurnPhase();

        yield return WaitForKeyPress();

        // NEW TURN
        ProceedToNewTurn();
    }

    private void InitiatingEndofTurnPhase()
    {
        currentPhaseText.text = "Ending Phase";
        turnOutcome.text = "Current turn has ended.";

        myCombatManager.EndOfTurnProcess();

        //TODO maybe add a pause here to process end turn in general       
    }

    private void ProceedToNewTurn()
    {
        processingTurnPanel.SetActive(false);
        turnCount++;
        turnCountText.text = "TURN " + turnCount;
        turnOutcome.text = "Choose a command.";
    }

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

    void TurnOffSkillUI()
    {
        playerSkillTextBoxAnimator.SetBool("SlideInWindow", false);
        enemySkillTextBoxAnimator.SetBool("SlideInWindow", false);
    }


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


    IEnumerator WaitForKeyPress()
    {
        bool isPressed = false;
        while (!isPressed)
        {
            if (Input.GetMouseButtonDown(0) && !combatEnded)
            {
                isPressed = true;

                // Turn off Skill UI upon key press
                TurnOffSkillUI();
            }
            yield return null;
        }
    }

    // Setter
    public void SetCombatHasEnded(bool status)
    {
        combatEnded = status;
    }
}
