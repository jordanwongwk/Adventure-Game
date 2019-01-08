using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/Damage or Heal")]
public class HealthPointSkills : SkillInfo {

    [Header("Skill Parameters")]
    [SerializeField] float damageMultiplier = 1.5f;
    [SerializeField] float damageToCaster = 0f;

    SkillType skillIdentity = SkillType.healthPointRelated;

    public override void SettingSkillType()
    {
        base.thisSkillType = skillIdentity;
        Debug.Log("Set HP Skills");
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
