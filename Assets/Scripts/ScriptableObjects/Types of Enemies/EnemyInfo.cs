using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemyInfo : ScriptableObject {

    [Header("Enemy Base Info")]
    [SerializeField] string enemyName;
    //[SerializeField] *Type* enemyType;
    [SerializeField] Sprite enemySprite;
    [SerializeField] RuntimeAnimatorController enemyAnimatorController;

    [Header("Enemy Stats")]
    [SerializeField] int level;
    [SerializeField] int healthPoints;
    [SerializeField] int strength;
    [SerializeField] int speed;
    [SerializeField] List<SkillEffect> thisEnemySpecialSkills;

    // Getter for Base Infos
    public string GetEnemyName()
    {
        return enemyName;
    }

    public Sprite GetEnemySprite()
    {
        return enemySprite;
    }

    public RuntimeAnimatorController GetEnemyAnimatorController()
    {
        return enemyAnimatorController;
    }


    // Getter for Enemy Stats
    public int GetEnemyLevel()
    {
        return level;
    }

    public int GetEnemyHealthPoints()
    {
        return healthPoints;
    }

    public int GetEnemyStrength()
    {
        return strength;
    }

    public int GetEnemySpeed()
    {
        return speed;
    }

    public int GetNumberOfEnemySpecialSkill()
    {
        return thisEnemySpecialSkills.Count;
    }

    public SkillEffect GetEnemySpecialSkill(int count)
    {
        return thisEnemySpecialSkills[count];
    }
}
