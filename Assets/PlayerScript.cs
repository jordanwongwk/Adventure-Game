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
    int initialHealth;
    int finalHealth;

    bool reducingHealth = false;

    CombatCommand chosenCommand;
    CombatManager myCombatManager;

    const float LERPING_SPEED = 2.0f;

    // Use this for initialization
    void Start()
    {
        myCombatManager = FindObjectOfType<CombatManager>();

        InitializingHealthAndHealthUI();
    }

    private void InitializingHealthAndHealthUI()
    {
        initialHealth = healthPoints;
        finalHealth = initialHealth;

        playerHealthText.text = initialHealth.ToString();
        float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
        playerHealthForeground.fillAmount = currentFill;
    }

    // Update is called once per frame
    void Update()
    {
        if (reducingHealth)
        {
            if (initialHealth == finalHealth) { reducingHealth = false; }
            initialHealth = (int)Mathf.Lerp(initialHealth, finalHealth, Time.deltaTime / LERPING_SPEED);
            playerHealthText.text = initialHealth.ToString();

            float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
            playerHealthForeground.fillAmount = currentFill;

            if (initialHealth <= 0)
            {
                initialHealth = 0;
                myCombatManager.EndOfCombat(gameObject);
            }
        }
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
        finalHealth -= thisTurnDamage;
        reducingHealth = true;
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
