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

    [SerializeField] Text turnCountText;
    [SerializeField] Text playerCommandText;

    int turnCount = 1;

	// Use this for initialization
	void Start ()
    {
        turnCountText.text = "TURN " + turnCount;
        playerCommandText.text = "Battle commence!";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickExecuteCommand(int commandInt)
    {
        // Cast Int to Enum
        CombatCommand chosenCommand = (CombatCommand)commandInt;
        switch (chosenCommand)
        {
            case CombatCommand.attack:
                playerCommandText.text = "Player attacks!";
                break;
            case CombatCommand.powerAttack:
                playerCommandText.text = "Player use power attack!";
                break;
            case CombatCommand.guard:
                playerCommandText.text = "Player guards!";
                break;
            default:
                Debug.Log("Invalid input");
                break;
        }

        turnCount++;
        turnCountText.text = "TURN " + turnCount;
    }
}
