using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    [Header("Character Stat")]
    [SerializeField] int maxHealthPoints = 100;
    [SerializeField] int strength = 2;
    [SerializeField] int defense = 2;
    [SerializeField] int speed = 10;

    [Header("Character UI")]
    [SerializeField] Text healthText;
    [SerializeField] Text nameText;
    [SerializeField] Image healthForeground;

    int initialStrength;
    int initialDefense;
    int initialSpeed;

    int thisTurnDamage = 0;
    int currentHealth;
    int resultHealth;

    CombatManager myCombatManager;
    Animator myAnimatorController;

    const float LERPING_SPEED = 3.0f;

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

        InitializingCharacterHealthAndHealthUI();
        InitializingCharacterStats();
    }

    private void SettingUpEnemy(EnemyInfo enemyEncountered)
    {
        myAnimatorController.runtimeAnimatorController = enemyEncountered.GetEnemyAnimatorController();

        nameText.text = "Lv " + enemyEncountered.GetEnemyLevel() + " " + enemyEncountered.GetEnemyName();

        maxHealthPoints = enemyEncountered.GetEnemyHealthPoints();
        strength = enemyEncountered.GetEnemyStrength();
        speed = enemyEncountered.GetEnemySpeed();
    }

    private void InitializingCharacterHealthAndHealthUI()
    {
        currentHealth = maxHealthPoints;
        resultHealth = currentHealth;

        healthText.text = currentHealth.ToString();
        float currentFill = (float)currentHealth / maxHealthPoints;        // Cast it to float 
        healthForeground.fillAmount = currentFill;
    }

    private void InitializingCharacterStats()
    {
        initialStrength = strength;
        initialDefense = defense;
        initialSpeed = speed;
    }


    public void ThisCharacterTakingDamage(int damage)
    {
        // TODO Manage defense here
        thisTurnDamage = damage - defense;
        if (thisTurnDamage <= 0) { thisTurnDamage = 0; }

        resultHealth -= thisTurnDamage;

        StartCoroutine(MovingHealthBar());
    }

    public void ThisCharacterHeals(int healAmount)
    {
        // TODO Heal Animation
        resultHealth += healAmount;
        if (resultHealth > maxHealthPoints) { resultHealth = maxHealthPoints; }

        StartCoroutine(MovingHealthBar());
    }

    IEnumerator MovingHealthBar()
    {
        float lerpTime = 0;

        while (resultHealth != currentHealth)
        {
            currentHealth = (int)Mathf.Lerp(currentHealth, resultHealth, lerpTime);
            lerpTime += Time.deltaTime * LERPING_SPEED;

            healthText.text = currentHealth.ToString();
            float currentFill = (float)currentHealth / maxHealthPoints;        // Cast it to float 
            healthForeground.fillAmount = currentFill;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                myCombatManager.EndOfCombat(gameObject);
                break;
            }
            yield return null;
        }
    }

    public void ThisCharacterBuff(int duration, float strMultiplier, float defMultiplier, float spdMultiplier)
    {
        // int newStr = (int)(strength * strMultiplier);
        // if (newStr > strength)
        // { 
        //      strength = newStr; 
        //      strBuffMultiplier = strMultiplier;      [Global Var]
        //      strBuffDurationLeft = duration;
        // }
        // Same goes for DEF and SPD
    }

    public void ThisCharacterDebuff(int duration, float strMultiplier, float defMultiplier, float spdMultiplier)
    {
        // int newStr = (int)(strength / strMultiplier);
        // if (newStr < strength)
        // { 
        //      strength = newStr; 
        //      strDebuffMultiplier = strMultiplier;      [Global Var]
        //      strDebuffDurationLeft = duration;
        // }
        // Same goes for DEF and SPD
    }

    // Check every turn end?
    // public void CheckForThisCharacterBuffDebuffDuration()
    // {
    //      if (strDebuffDurationLeft == 1)
    //      {
    //          debuff[0] = false;
    //          strength *= strDebuffMultiplier;
    //      }
    //      strDebuffDurationLeft--;
    // }

    public void ThisCharacterStatReset()
    {
        strength = initialStrength;
        defense = initialDefense;
        speed = initialSpeed;
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

    public int GetThisCharDefense()
    {
        return defense;
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
