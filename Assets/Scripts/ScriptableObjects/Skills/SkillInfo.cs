using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class SkillInfo : ScriptableObject {

    [Header("Skill Parameters")]
    [SerializeField] string skillName;
    [SerializeField] float damageMultiplier;
    [SerializeField] float damageMitigation;
    [SerializeField] GameObject target;
    [SerializeField] ActivationTime skillActivationTime;
    // TODO [SerializeField] CombatCommand commandThatTriggers;
    [SerializeField] SkillEffect thisSkillEffectScript;

    [Header("RNG")]
    [SerializeField] float minActivateRNG;
    [SerializeField] float maxActivateRNG;

    // Getter
    public string GetSkillName()
    {
        return skillName;
    }

    public float GetSkillDamageMultiplier()
    {
        return damageMultiplier;
    }

    public ActivationTime GetSkillActivationTime()
    {
        return skillActivationTime;
    }

    public float GetMinActivationRNG()
    {
        return minActivateRNG;
    }

    public float GetMaxActivationRNG()
    {
        return maxActivateRNG;
    }
}
