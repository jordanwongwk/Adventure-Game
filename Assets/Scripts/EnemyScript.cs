using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Info")]
    [SerializeField] EnemyInfo enemyEncountered;

    [Header("Enemy UI")]
    [SerializeField] Text enemyHealthText;
    [SerializeField] Text enemyNameUI;
    [SerializeField] Image enemyHealthForeground;

    // Enemy Info
    int healthPoints;
    int strength;
    int speed;

    int thisTurnDamage = 0;
    int initialHealth;
    int finalHealth;

    bool reducingHealth = false;

    CombatCommand chosenEnemyCommand;
    Animator myAnimatorController;
    CombatManager myCombatManager;
    EnemySkillScript mySkillScript;

    const float LERPING_SPEED = 2.0f;

    // Use this for initialization
    void Start ()
    {
        myAnimatorController = GetComponent<Animator>();
        myCombatManager = FindObjectOfType<CombatManager>();
        mySkillScript = GetComponent<EnemySkillScript>();
        SettingUpEnemy();
        InitializingHealthAndHealthUI();
    }

    private void SettingUpEnemy()
    {
        GetComponent<SpriteRenderer>().sprite = enemyEncountered.GetEnemySprite();

        myAnimatorController.runtimeAnimatorController = enemyEncountered.GetEnemyAnimatorController();

        enemyNameUI.text = "Lv " + enemyEncountered.GetEnemyLevel() + " " + enemyEncountered.GetEnemyName();

        healthPoints = enemyEncountered.GetEnemyHealthPoints();
        strength = enemyEncountered.GetEnemyStrength();
        speed = enemyEncountered.GetEnemySpeed();
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

        mySkillScript.AttemptToActivateSkill();
    }

    public void EnemyTakingDamage(int damage)
    {
        // Manage defense here
        thisTurnDamage = damage - 2;
        finalHealth -= thisTurnDamage;
        reducingHealth = true;
    }

    // Public Functions
    public void EnemyAttemptToUseSkill(ActivationTime currentTime)
    {
        mySkillScript.AttemptToUseSkill(currentTime);
    }

    public void EnemyAttackAnimation()
    {
        myAnimatorController.SetTrigger("EnemyAttack");
    }

    public void EnemyGuardPowerAttackAnimation()
    {
        myAnimatorController.SetTrigger("EnemyGuard");
    }

    // Getter and Setter
    public CombatCommand GetChosenEnemyCommand()
    {
        return chosenEnemyCommand;
    }

    public EnemyInfo GetThisEnemyInfo()
    {
        return enemyEncountered;
    }

    public string GetEnemyName()
    {
        return enemyEncountered.GetEnemyName();
    }

    public int GetEnemyStrength()
    {
        return strength;
    }

    public int GetEnemySpeed()
    {
        return speed;
    }

    public int GetThisTurnEnemyDamage()
    {
        return thisTurnDamage;
    }
}
