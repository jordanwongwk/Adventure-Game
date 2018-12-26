using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Stat")]
    [SerializeField] int healthPoints = 100;
    [SerializeField] int strength = 2;

    [Header("Player UI")]
    [SerializeField] Text playerHealthText;
    [SerializeField] Image playerHealthForeground;

    int thisTurnDamage = 0;
    int currentHealth;

    CombatCommand chosenCommand;
    CombatManager myCombatManager;

    // Use this for initialization
    void Start()
    {
        myCombatManager = FindObjectOfType<CombatManager>();

        currentHealth = healthPoints;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickExecuteCommand(int commandInt)
    {
        chosenCommand = (CombatCommand)commandInt;      // Cast Int to Enum
        myCombatManager.ProgressCurrentTurn();
    }

    public void PlayerResolveDamage(int damage)
    {
        // Manage defense here
        thisTurnDamage = damage - 2;
        currentHealth -= thisTurnDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            myCombatManager.EndOfCombat(gameObject);
        }

        UpdateHealth();
    }

    void UpdateHealth()
    {
        playerHealthText.text = currentHealth.ToString();
        float currentFill = (float)currentHealth / healthPoints;        // Cast it to float 
        playerHealthForeground.fillAmount = currentFill;
    }

    // Getter and Setter
    public CombatCommand GetPlayerChosenCommand()
    {
        return chosenCommand;
    }

    public int GetPlayerStrength()
    {
        return strength;
    }

    public int GetThisTurnPlayerDamage()
    {
        return thisTurnDamage;
    }
}
