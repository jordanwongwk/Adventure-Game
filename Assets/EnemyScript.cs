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
    int initialHealth;
    int finalHealth;

    bool reducingHealth = false;

    CombatCommand chosenEnemyCommand;
    CombatManager myCombatManager;

    const float LERPING_SPEED = 2.0f;

    // Use this for initialization
    void Start () {
        myCombatManager = FindObjectOfType<CombatManager>();

        InitializingHealthAndHealthUI();
    }

    private void InitializingHealthAndHealthUI()
    {
        initialHealth = healthPoints;
        finalHealth = initialHealth;

        enemyHealthText.text = initialHealth.ToString();
        float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
        enemyHealthForeground.fillAmount = currentFill;
    }

    // Update is called once per frame
    void Update ()
    {
        if (reducingHealth)
        {
            if (initialHealth == finalHealth) { reducingHealth = false; }
            initialHealth = (int)Mathf.Lerp(initialHealth, finalHealth, Time.deltaTime / LERPING_SPEED);
            enemyHealthText.text = initialHealth.ToString();

            float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
            enemyHealthForeground.fillAmount = currentFill;

            if (initialHealth <= 0)
            {
                initialHealth = 0;
                myCombatManager.EndOfCombat(gameObject);
            }
        }
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
        finalHealth -= thisTurnDamage;
        reducingHealth = true;
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
