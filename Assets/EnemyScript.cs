using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stat")]
    [SerializeField] int healthPoints = 50;
    [SerializeField] int strength = 2;

    [Header("Enemy UI")]
    [SerializeField] Text enemyHealthText;
    [SerializeField] Image enemyHealthForeground;

    int thisTurnDamage = 0;
    int currentHealth;

    CombatCommand chosenEnemyCommand;
    CombatManager myCombatManager;

	// Use this for initialization
	void Start () {
        myCombatManager = FindObjectOfType<CombatManager>();

        currentHealth = healthPoints;
        UpdateHealth();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PickingEnemyCommand()
    {
        int generateNumber = Random.Range(1, 300);

        if (generateNumber % 3 == 0)
        {
            chosenEnemyCommand = CombatCommand.attack;
        }
        else if (generateNumber % 3 == 1)
        {
            chosenEnemyCommand = CombatCommand.powerAttack;
        }
        else
        {
            chosenEnemyCommand = CombatCommand.guard;
        }
    }

    public void EnemyResolveDamage(int damage)
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
        enemyHealthText.text = currentHealth.ToString();
        float currentFill = (float)currentHealth / healthPoints;        // Cast it to float 
        enemyHealthForeground.fillAmount = currentFill;
    }


    // Getter and Setter
    public CombatCommand GetChosenEnemyCommand()
    {
        return chosenEnemyCommand;
    }

    public int GetEnemyStrength()
    {
        return strength;
    }

    public int GetThisTurnEnemyDamage()
    {
        return thisTurnDamage;
    }
}
