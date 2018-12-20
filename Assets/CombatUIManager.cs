using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour {
    [Header("Combat UI")]
    [SerializeField] Text turnCountText;
    [SerializeField] Text playerCommandText;
    [SerializeField] Text enemyCommandText;
    [SerializeField] Text turnOutcome;

    [Header("End of Combat UI")]
    [SerializeField] GameObject endofCombatPanel;
    [SerializeField] Text combatOutcome;

    [Header("Other Settings")]
    [SerializeField] List<Button> commandButtons = new List<Button>();

    PlayerScript playerObjectScript;
    EnemyScript enemyObjectScript;

    int turnCount = 0;

    // Use this for initialization
    void Start () {
        playerObjectScript = FindObjectOfType<PlayerScript>();
        enemyObjectScript = FindObjectOfType<EnemyScript>();

        EndOfTurnProcess();
        playerCommandText.text = "Battle commence!";
        enemyCommandText.text = "Battle commence!";
    }

    public void ManageCommandButton(bool isButtonReady)
    {
        foreach (Button button in commandButtons)
        {
            button.interactable = isButtonReady;
        }
    }

    public void EndOfTurnProcess()
    {
        ManageCommandButton(true);
        turnCount++;
        turnCountText.text = "TURN " + turnCount;
    }

    // Managing Texts
    public void ThisTurnCombatOutcome(CombatCommand playerCommand, CombatCommand enemyCommand)
    {
        int playerDamageSufferedThisTurn = playerObjectScript.GetThisTurnPlayerDamage();
        int enemyDamageSufferedThisTurn = enemyObjectScript.GetThisTurnEnemyDamage();

        switch (playerCommand)
        {
            case CombatCommand.attack:
                playerCommandText.text = "Player attacks!";

                if (enemyCommand == CombatCommand.attack)
                {
                    enemyCommandText.text = "The enemy attacks!";
                    turnOutcome.text = "Player deals " + enemyDamageSufferedThisTurn + " to enemy!\nEnemy deals " + playerDamageSufferedThisTurn + " to player!";
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    enemyCommandText.text = "The enemy uses power attack!";
                    turnOutcome.text = "Player deals " + enemyDamageSufferedThisTurn + " to enemy!\nEnemy deals " + playerDamageSufferedThisTurn + " to player!";
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    enemyCommandText.text = "The enemy guards!";
                    turnOutcome.text = "Enemy guards!\nPlayer penetrates, dealing " + enemyDamageSufferedThisTurn + " to enemy!";
                }
                break;

            case CombatCommand.powerAttack:
                playerCommandText.text = "Player uses power attack!";

                if (enemyCommand == CombatCommand.attack)
                {
                    enemyCommandText.text = "The enemy attacks!";
                    turnOutcome.text = "Player deals " + enemyDamageSufferedThisTurn + " to enemy!\nEnemy deals " + playerDamageSufferedThisTurn + " to player!";
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    enemyCommandText.text = "The enemy uses power attack!";
                    turnOutcome.text = "Player deals " + enemyDamageSufferedThisTurn + " to enemy!\nEnemy deals " + playerDamageSufferedThisTurn + " to player!";
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    enemyCommandText.text = "The enemy guards!";
                    turnOutcome.text = "Enemy guards!\nPlayer's power attack has been evaded!";
                }
                break;

            case CombatCommand.guard:
                playerCommandText.text = "Player guards!";

                if (enemyCommand == CombatCommand.attack)
                {
                    enemyCommandText.text = "The enemy attacks!";
                    turnOutcome.text = "Player guards!\nEnemy penetrates, dealing " + playerDamageSufferedThisTurn + " to player!";
                }
                else if (enemyCommand == CombatCommand.powerAttack)
                {
                    enemyCommandText.text = "The enemy uses power attack!";
                    turnOutcome.text = "Player guards!\nEnemy's power attack has been evaded!";
                }
                else if (enemyCommand == CombatCommand.guard)
                {
                    enemyCommandText.text = "The enemy guards!";
                    turnOutcome.text = "Player guards!\nEnemy guards!";
                }
                break;

            default:
                Debug.LogError("Error processing combat outcome.");
                break;
        }
    }

    public void EndOfCombatResult(GameObject defeatedCharacter)
    {
        endofCombatPanel.SetActive(true);

        // TODO Please address the issue of draw. Player should win the moment the last attack lands!
        if (defeatedCharacter == playerObjectScript.gameObject)
        {
            combatOutcome.text = "Player has been defeated!\nYou lost!";
        }
        else
        {
            combatOutcome.text = "Enemy has been defeated!\nYou win!";
        }
    }
}
