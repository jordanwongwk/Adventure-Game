using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/Stat Change")]
public class BuffingSkills : SkillInfo {

    [Header("Skill Parameteres")]
    [SerializeField] int duration = 1;

    [Header("Skill Parameteres : Buffs")]
    [SerializeField] float strengthBuffMultiplier = 1f;
    [SerializeField] float defenseBuffMultiplier = 1f;
    [SerializeField] float speedBuffMultiplier = 1f;

    [Header("Skill Parameteres : Debuffs")]
    [SerializeField] float strengthDebuffMultiplier = 1f;
    [SerializeField] float defenseDebuffMultiplier = 1f;
    [SerializeField] float speedDebuffMultiplier = 1f;

    [Header("Buff Outcome Text")]
    [SerializeField] string outcomeTextForBuffDebuff = "";

    SkillType skillIdentity = SkillType.Buffing;

    public override void SettingSkillType()
    {
        base.thisSkillType = skillIdentity;
    }

    // General
    public int GetBuffDebuffDuration()
    {
        return duration;
    }

    public string GetOutcomeTextForBuffDebuff()
    {
        return outcomeTextForBuffDebuff;
    }

    // Buff
    public float GetStrengthBuffMultiplier()
    {
        return strengthBuffMultiplier;
    }
    
    public float GetDefenseBuffMultiplier()
    {
        return defenseBuffMultiplier;
    }

    public float GetSpeedBuffMultiplier()
    {
        return speedBuffMultiplier;
    }

    // Debuff
    public float GetStrengthDebuffMultiplier()
    {
        return strengthDebuffMultiplier;
    }

    public float GetDefenseDebuffMultiplier()
    {
        return defenseDebuffMultiplier;
    }

    public float GetSpeedDebuffMultiplier()
    {
        return speedDebuffMultiplier;
    }
}
