using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Damage, Heal, Buffing }
public enum Target { Self, Opponent }

public abstract class SkillInfo : ScriptableObject {

    [Header("Skill Information")]
    [SerializeField] string skillName;
    [SerializeField] ActivationTime skillActivationTime;
    [SerializeField] CombatCommand commandThatTriggers;
    [SerializeField] Target skillTarget;
    [SerializeField] CombatCommand opponentCommandThatTriggers;
    [SerializeField] GameObject skillParticleEffect;
    [SerializeField] AudioClip skillAudioClip;

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

    public Target GetSkillTarget()
    {
        return skillTarget;
    }

    public CombatCommand GetOpponentCommandThatTriggers()
    {
        return opponentCommandThatTriggers;
    }

    public GameObject GetSkillParticleEffect()
    {
        return skillParticleEffect;
    }

    public AudioClip GetSkillAudioClip()
    {
        return skillAudioClip;
    }

    public SkillType GetSkillType()
    {
        return thisSkillType;
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
