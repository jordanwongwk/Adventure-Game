using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/Heal")]
public class HealSkills : SkillInfo {

    [Header("Skill Parameters")]
    [SerializeField] int healAmountMin = 0;
    [SerializeField] int healAmountMax = 10;

    SkillType skillIdentity = SkillType.Heal;

    public override void SettingSkillType()
    {
        base.thisSkillType = skillIdentity;
    }

    public int GetRandomHealAmount()
    {
        int healAmount = Random.Range(healAmountMin, healAmountMax);
        return healAmount;
    }
}
