using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class SkillEffect : ScriptableObject {

    [Header("Skill Parameters")]
    [SerializeField] string skillName;
    [SerializeField] float damageDealt;
    [SerializeField] float damageMitigation;
    [SerializeField] GameObject target;
    [SerializeField] ActivationTime skillActivationTime;
    // TODO [SerializeField] CombatCommand commandThatTriggers;

    [Header("RNG")]
    [SerializeField] float minActivateRNG;
    [SerializeField] float maxActivateRNG;


    public string GetSkillName()
    {
        return skillName;
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
