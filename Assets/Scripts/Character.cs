using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    [Header("Character Stat")]
    [SerializeField] int healthPoints = 100;
    [SerializeField] int strength = 2;
    [SerializeField] int speed = 10;

    [Header("Character UI")]
    [SerializeField] Text healthText;
    [SerializeField] Text nameText;
    [SerializeField] Image healthForeground;

    int thisTurnDamage = 0;
    int initialHealth;
    int finalHealth;
    bool reducingHealth = false;

    CombatManager myCombatManager;
    Animator myAnimatorController;

    const float LERPING_SPEED = 2.0f;

    // Use this for initialization
    void Start ()
    {
        myCombatManager = FindObjectOfType<CombatManager>();
        myAnimatorController = GetComponent<Animator>();

        if (GetComponent<EnemyScript>())
        {
            EnemyInfo thisEnemyInfo = GetComponent<EnemyScript>().GetThisEnemyInfo();
            SettingUpEnemy(thisEnemyInfo);
        }

        InitializingHealthAndHealthUI();
    }

    private void SettingUpEnemy(EnemyInfo enemyEncountered)
    {
        myAnimatorController.runtimeAnimatorController = enemyEncountered.GetEnemyAnimatorController();

        nameText.text = "Lv " + enemyEncountered.GetEnemyLevel() + " " + enemyEncountered.GetEnemyName();

        healthPoints = enemyEncountered.GetEnemyHealthPoints();
        strength = enemyEncountered.GetEnemyStrength();
        speed = enemyEncountered.GetEnemySpeed();
    }

    private void InitializingHealthAndHealthUI()
    {
        initialHealth = healthPoints;
        finalHealth = initialHealth;

        healthText.text = initialHealth.ToString();
        float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
        healthForeground.fillAmount = currentFill;
    }

    // Update is called once per frame
    void Update ()
    {
        if (reducingHealth)
        {
            if (initialHealth == finalHealth) { reducingHealth = false; }
            initialHealth = (int)Mathf.Lerp(initialHealth, finalHealth, Time.deltaTime / LERPING_SPEED);
            healthText.text = initialHealth.ToString();

            float currentFill = (float)initialHealth / healthPoints;        // Cast it to float 
            healthForeground.fillAmount = currentFill;

            if (initialHealth <= 0)
            {
                initialHealth = 0;
                myCombatManager.EndOfCombat(gameObject);
            }
        }
    }

    public void ThisCharacterTakingDamage(int damage)
    {
        // Manage defense here
        thisTurnDamage = damage - 2;
        finalHealth -= thisTurnDamage;
        reducingHealth = true;
    }


    public void AttackAnimation()
    {
        myAnimatorController.SetTrigger("Attack");
    }

    public void GuardPowerAttackAnimation()
    {
        myAnimatorController.SetTrigger("Guard");
    }

    public int GetThisCharStrength()
    {
        return strength;
    }

    public int GetThisCharSpeed()
    {
        return speed;
    }

    public int GetThisTurnCharDamage()
    {
        return thisTurnDamage;
    }
}
