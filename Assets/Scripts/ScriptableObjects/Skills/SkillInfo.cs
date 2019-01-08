using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { healthPointRelated, buffing }

public abstract class SkillInfo : ScriptableObject {

    [Header("Skill Information")]
    [SerializeField] string skillName;
    [SerializeField] GameObject target;
    [SerializeField] ActivationTime skillActivationTime;
    [SerializeField] CombatCommand commandThatTriggers;

    [Header("RNG")]
    [SerializeField] float minActivateRNG;
    [SerializeField] float maxActivateRNG;

    protected SkillType thisSkillType;

    // Use abstract and override to each set their own type into the info
    public abstract void SettingSkillType();

    // Getter
    public string GetSkillName()
    {
        return skillName;
    }

    public ActivationTime GetSkillActivationTime()
    {
        return skillActivationTime;
    }

    public CombatCommand GetCommandThatTriggersSkill()
    {
        return commandThatTriggers;
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
