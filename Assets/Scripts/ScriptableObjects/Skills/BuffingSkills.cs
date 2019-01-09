using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/Buffing or Debuffing")]
public class BuffingSkills : SkillInfo {

    [Header("Skill Parameters: Buffs")]
    [SerializeField] float strengthBuffMultiplier = 1.5f;

    [Header("Skill Parameters: Debuffs")]
    [SerializeField] float strengthDebuffMultiplier = 1.5f;
    // Still got DEF and SPD etc.

    SkillType skillIdentity = SkillType.Buffing;

    public override void SettingSkillType()
    {
        base.thisSkillType = skillIdentity;
    }

    public float GetStrengthBuffMultiplier()
    {
        return strengthBuffMultiplier;
    }

    public float GetStrengthDebuffMultiplier()
    {
        return strengthDebuffMultiplier;
    }
}
