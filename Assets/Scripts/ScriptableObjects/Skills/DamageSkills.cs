using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/Damage")]
public class DamageSkills : SkillInfo {

    [Header("Skill Parameters")]
    [SerializeField] float damageMultiplier = 1.5f;
    [SerializeField] float damageToCaster = 0f;

    SkillType skillIdentity = SkillType.Damage;

    public override void SettingSkillType()
    {
        base.thisSkillType = skillIdentity;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public float GetDamageToCaster()
    {
        return damageToCaster;
    }
}
