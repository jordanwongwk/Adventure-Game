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

    [Header("Character SFX")]
    [SerializeField] AudioClip attackSFX;

    int thisTurnDamage = 0;
    int currentHealth;
    int resultHealth;

    List<float> statList = new List<float>();
    List<int> buffDuration = new List<int>();
    List<int> debuffDuration = new List<int>();
    List<float> buffMultiplier = new List<float>();
    List<float> debuffMultiplier = new List<float>();

    AudioSource myAudioSource;
    CombatManager myCombatManager;
    CombatUIManager myCombatUIManager;
    Animator myAnimatorController;

    const float LERPING_SPEED = 3.0f;
    const int STRENGTH_INDEX = 0;
    const int DEFENSE_INDEX = 1;
    const int SPEED_INDEX = 2;

    // Use this for initialization
    void Start ()
    {
        myAudioSource = GetComponentInChildren<AudioSource>();
        myCombatManager = FindObjectOfType<CombatManager>();
        myCombatUIManager = FindObjectOfType<CombatUIManager>();
        myAnimatorController = GetComponent<Animator>();

        if (GetComponent<EnemyScript>())
        {
            EnemyInfo thisEnemyInfo = GetComponent<EnemyScript>().GetThisEnemyInfo();
            SettingUpEnemy(thisEnemyInfo);
        }

        InitializingCharacterHealthAndHealthUI();
        InitializingCharacterStats();
        InitializingCharacterBuffDebuffList();
    }

    private void SettingUpEnemy(EnemyInfo enemyEncountered)
    {
        myAnimatorController.runtimeAnimatorController = enemyEncountered.GetEnemyAnimatorController();

        nameText.text = "Lv " + enemyEncountered.GetEnemyLevel() + " " + enemyEncountered.GetEnemyName();

        maxHealthPoints = enemyEncountered.GetEnemyHealthPoints();
        strength = enemyEncountered.GetEnemyStrength();
        defense = enemyEncountered.GetEnemyDefense();
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
        statList.Add(strength);
        statList.Add(defense);
        statList.Add(speed);
    }

    private void InitializingCharacterBuffDebuffList()
    {
        for (int i = 0; i < 3; i++)
        {
            buffDuration.Add(0);
            debuffDuration.Add(0);
            buffMultiplier.Add(1.0f);
            debuffMultiplier.Add(1.0f);
        }

        myCombatUIManager.SetCharacterStatDurationText(this, buffDuration, debuffDuration);
    }


    public void ThisCharacterTakingDamage(int damage)
    {
        // TODO Manage defense here, future add armor from item
        int currentDefense = Mathf.RoundToInt(statList[DEFENSE_INDEX]);
        thisTurnDamage = damage - currentDefense;
        if (thisTurnDamage <= 0) { thisTurnDamage = 0; }

        resultHealth -= thisTurnDamage;

        StartCoroutine(MovingHealthBar());
    }

    public void ThisCharacterHeals(int healAmount)
    {
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
        if (strMultiplier > buffMultiplier[STRENGTH_INDEX])
        {
            statList[STRENGTH_INDEX] *= strMultiplier; 
            buffMultiplier[STRENGTH_INDEX] = strMultiplier;
            buffDuration[STRENGTH_INDEX] = duration;
        }

        if (defMultiplier > buffMultiplier[DEFENSE_INDEX])
        {
            statList[DEFENSE_INDEX] *= defMultiplier;
            buffMultiplier[DEFENSE_INDEX] = defMultiplier;
            buffDuration[DEFENSE_INDEX] = duration;
        }

        if (spdMultiplier > buffMultiplier[SPEED_INDEX])
        {
            statList[SPEED_INDEX] *= spdMultiplier; 
            buffMultiplier[SPEED_INDEX] = spdMultiplier;
            buffDuration[SPEED_INDEX] = duration;
        }

        myCombatUIManager.SetCharacterStatDurationText(this, buffDuration, debuffDuration);
    }

    public void ThisCharacterDebuff(int duration, float strMultiplier, float defMultiplier, float spdMultiplier)
    {
        if (strMultiplier > debuffMultiplier[STRENGTH_INDEX])
        {
            statList[STRENGTH_INDEX] /= strMultiplier; 
            debuffMultiplier[STRENGTH_INDEX] = strMultiplier;
            debuffDuration[STRENGTH_INDEX] = duration;
        }

        if (defMultiplier > debuffMultiplier[DEFENSE_INDEX])
        {
            statList[DEFENSE_INDEX] /= defMultiplier; 
            debuffMultiplier[DEFENSE_INDEX] = defMultiplier;
            debuffDuration[DEFENSE_INDEX] = duration;
        }

        if (spdMultiplier > debuffMultiplier[SPEED_INDEX])
        {
            statList[SPEED_INDEX] /= spdMultiplier;
            debuffMultiplier[SPEED_INDEX] = spdMultiplier;
            debuffDuration[SPEED_INDEX] = duration;
        }

        myCombatUIManager.SetCharacterStatDurationText(this, buffDuration, debuffDuration);
    }

    // Check every turn end?
    public void CheckForThisCharacterBuffDebuffDuration()
    {
        for (int i = 0; i < buffDuration.Count; i++)
        {
            if (buffDuration[i] > 0)
            {
                if (buffDuration[i] == 1)
                {
                    statList[i] = statList[i] / buffMultiplier[i];
                    buffMultiplier[i] = 1.0f;
                }

                buffDuration[i]--;
            } 
        }

        for (int j = 0; j < debuffDuration.Count; j++)
        {
            if (debuffDuration[j] > 0)
            {
                if (debuffDuration[j] == 1)
                {
                    statList[j] = statList[j] * debuffMultiplier[j];
                    debuffMultiplier[j] = 1.0f;
                }

                debuffDuration[j]--;
            }
        }

        myCombatUIManager.SetCharacterStatDurationText(this, buffDuration, debuffDuration);
    }


    public void AttackAnimation()
    {
        myAnimatorController.SetTrigger("Attack");
    }

    public void PlayAttackSFX()
    {
        myAudioSource.PlayOneShot(attackSFX);
    }

    public void GuardPowerAttackAnimation()
    {
        myAnimatorController.SetTrigger("Guard");
    }


    // Getter
    public int GetThisCharStrength()
    {
        return Mathf.RoundToInt(statList[STRENGTH_INDEX]);
    }

    public int GetThisCharDefense()
    {
        return Mathf.RoundToInt(statList[DEFENSE_INDEX]);
    }

    public int GetThisCharSpeed()
    {
        return Mathf.RoundToInt(statList[SPEED_INDEX]);
    }

    public int GetThisTurnCharDamage()
    {
        return thisTurnDamage;
    }
}
