﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationTime { beforeCombat, duringCombatOffense, duringCombatDefense, afterCombat }

public class SkillScript : MonoBehaviour {

    [SerializeField] List<SkillInfo> thisCharacterSkills = new List<SkillInfo>();
    [SerializeField] GameObject skillEffectObject;

    bool charIsUsingSkill = false;

    Character thisCharacter;
    Character thisCharacterOpponent;
    CombatUIManager myCombatUIManager;
    AudioSource skillAudioSource;

    // Use this for initialization
    void Start ()
    {
        myCombatUIManager = FindObjectOfType<CombatUIManager>();
        thisCharacter = GetComponent<Character>();
        skillAudioSource = skillEffectObject.GetComponent<AudioSource>();

        // If its enemy
        if (GetComponent<EnemyScript>() != null)
        {
            SetupEnemySkills();
        }
        else
        {
            ClearEmptyPlayerSkillSlots();
        }

        // Setting every skill to designated types
        foreach (SkillInfo skill in thisCharacterSkills)
        {
            skill.SettingSkillType();
        }
    }

    private void SetupEnemySkills()
    {
        EnemyInfo thisEnemyInfo = GetComponent<EnemyScript>().GetThisEnemyInfo();

        for (int i = 0; i < thisEnemyInfo.GetNumberOfEnemySpecialSkill(); i++)
        {
            thisCharacterSkills.Add(thisEnemyInfo.GetEnemySpecialSkill(i));
        }
    }

    private void ClearEmptyPlayerSkillSlots()
    {
        for (int i = 0; i < thisCharacterSkills.Count; i++)
        {
            if (thisCharacterSkills[i] == null)
            {
                thisCharacterSkills.RemoveAt(i);
                i--;
            }
        }
    }

    // Setter
    public void SetThisCharacterOpponent(Character thisCharOppo)
    {
        thisCharacterOpponent = thisCharOppo;
    }

    public void SetThisCharacterUsingSkill(bool status)
    {
        charIsUsingSkill = status;
    }

    // Getter
    public bool GetIsThisCharacterUsingSkill()
    {
        return charIsUsingSkill;
    }


    public void AttemptToUseSkill(ActivationTime currentTime, CombatCommand unitCommand)
    {
        float randomValue = Random.Range(0f, 100f);

        switch (currentTime)
        {
            case ActivationTime.beforeCombat:
                for (int i = 0; i < thisCharacterSkills.Count; i++)
                {
                    if (thisCharacterSkills[i].GetMinActivationRNG() < randomValue && thisCharacterSkills[i].GetMaxActivationRNG() >= randomValue)
                    {
                        if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }

                        ProcessThisCharacterSkillActivation(currentTime, i);
                        break;
                    }
                }
                break;

            case ActivationTime.duringCombatOffense:
                for (int i = 0; i < thisCharacterSkills.Count; i++)
                {
                    if (thisCharacterSkills[i].GetMinActivationRNG() < randomValue && thisCharacterSkills[i].GetMaxActivationRNG() >= randomValue)
                    {
                        if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }
                        if (thisCharacterSkills[i].GetCommandThatTriggersSkill() != unitCommand) { continue; }

                        ProcessThisCharacterSkillActivation(currentTime, i);
                        break;
                    }
                }
                break;

            // Dealing with all the counters and retaliates
            case ActivationTime.duringCombatDefense:
                for (int i = 0; i < thisCharacterSkills.Count; i++)
                {
                    if (thisCharacterSkills[i].GetMinActivationRNG() < randomValue && thisCharacterSkills[i].GetMaxActivationRNG() >= randomValue)
                    {
                        if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }
                        // Like if enemy attack and you guard, still can counter. If enemy guard, you counter, thats a bit odd
                        if (thisCharacterSkills[i].GetCommandThatTriggersSkill() != unitCommand) { continue; }

                        ProcessThisCharacterSkillActivation(currentTime, i);
                        break;
                    }
                }
                break;

            case ActivationTime.afterCombat:
                for (int i = 0; i < thisCharacterSkills.Count; i++)
                {
                    if (thisCharacterSkills[i].GetMinActivationRNG() < randomValue && thisCharacterSkills[i].GetMaxActivationRNG() >= randomValue)
                    {
                        if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }

                        ProcessThisCharacterSkillActivation(currentTime, i);
                        break;
                    }
                }
                break;

            default:
                Debug.LogError("Error in processing skill.");
                break;
        }
    }

    private void ProcessThisCharacterSkillActivation(ActivationTime currentTime, int skillNumber)
    {
        charIsUsingSkill = true;
        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[skillNumber].GetSkillName());
        SkillType thisSkillType = thisCharacterSkills[skillNumber].GetSkillType();

        switch (thisSkillType)
        {
            case SkillType.Damage:
                ProcessDamageSkill(thisCharacterSkills[skillNumber]);
                break;

            case SkillType.Heal:
                ProcessHealingSkill(thisCharacterSkills[skillNumber]);
                break;

            case SkillType.Buffing:
                ProcessStatChangeSkill(thisCharacterSkills[skillNumber]);
                break;

            default:
                Debug.LogError("Error in processing " + thisCharacterSkills[skillNumber].GetSkillName() + " at " + currentTime.ToString());
                break;
        }
    }


    // Skill processes functions:
    private void ProcessDamageSkill(SkillInfo thisSkill)
    {
        float totalDamage = (thisSkill as DamageSkills).GetDamageMultiplier() * thisCharacter.GetThisCharStrength();
        Debug.Log(thisSkill.GetSkillName() + " Damage Dealt: " + totalDamage);

        Target skillTarget = thisSkill.GetSkillTarget();

        switch (skillTarget)
        {
            case Target.Self:
                thisCharacter.ThisCharacterTakingDamage(Mathf.RoundToInt(totalDamage));
                break;

            case Target.Opponent:
                thisCharacter.AttackAnimation();
                thisCharacterOpponent.ThisCharacterTakingDamage(Mathf.RoundToInt(totalDamage));
                myCombatUIManager.SetTurnOutcomeText(thisCharacter.name + " uses " + thisSkill.GetSkillName() + "!\nDealing " + 
                                                     thisCharacterOpponent.GetThisTurnCharDamage() + " to " + thisCharacterOpponent.name + "!");
                break;

            default:
                Debug.LogError("Can't log target for skill " + thisSkill.GetSkillName());
                break;
        }
    }

    private void ProcessHealingSkill(SkillInfo thisSkill)
    {
        int healAmount = (thisSkill as HealSkills).GetRandomHealAmount();

        GameObject skillParticleEffect = thisSkill.GetSkillParticleEffect();
        AudioClip skillAudioClip = thisSkill.GetSkillAudioClip();
        ActivateSkillEffects(skillParticleEffect, skillAudioClip);

        thisCharacter.ThisCharacterHeals(healAmount);
        myCombatUIManager.SetTurnOutcomeText(thisCharacter.name + " uses " + thisSkill.GetSkillName() + "!\nRecovering self for " + healAmount + "hp!");
    }

    private void ProcessStatChangeSkill(SkillInfo thisSkill)
    {
        Target targetChar = thisSkill.GetSkillTarget();
        GameObject statChangeParticleEffect = thisSkill.GetSkillParticleEffect();
        AudioClip skillAudioClip = thisSkill.GetSkillAudioClip();

        int skillDuration = (thisSkill as BuffingSkills).GetBuffDebuffDuration();
        string skillOutcomeText = (thisSkill as BuffingSkills).GetOutcomeTextForBuffDebuff();

        float strBuffMultiplier = (thisSkill as BuffingSkills).GetStrengthBuffMultiplier();
        float defBuffMultiplier = (thisSkill as BuffingSkills).GetDefenseBuffMultiplier();
        float spdBuffMultiplier = (thisSkill as BuffingSkills).GetSpeedBuffMultiplier();

        float strDebuffMultiplier = (thisSkill as BuffingSkills).GetStrengthDebuffMultiplier();
        float defDebuffMultiplier = (thisSkill as BuffingSkills).GetDefenseDebuffMultiplier();
        float spdDebuffMultiplier = (thisSkill as BuffingSkills).GetSpeedDebuffMultiplier();

        myCombatUIManager.SetTurnOutcomeText(thisCharacter.name + " uses " + thisSkill.GetSkillName() + "!\n" + skillOutcomeText);

        switch (targetChar)
        {
            case Target.Self:
                ActivateSkillEffects(statChangeParticleEffect, skillAudioClip);
                thisCharacter.ThisCharacterBuff(skillDuration, strBuffMultiplier, defBuffMultiplier, spdBuffMultiplier);
                thisCharacter.ThisCharacterDebuff(skillDuration, strDebuffMultiplier, defDebuffMultiplier, spdDebuffMultiplier);
                break;

            case Target.Opponent:
                thisCharacterOpponent.GetComponent<SkillScript>().ActivateSkillEffects(statChangeParticleEffect, skillAudioClip);
                thisCharacterOpponent.ThisCharacterBuff(skillDuration, strBuffMultiplier, defBuffMultiplier, spdBuffMultiplier);
                thisCharacterOpponent.ThisCharacterDebuff(skillDuration, strDebuffMultiplier, defDebuffMultiplier, spdDebuffMultiplier);
                break;

            default:
                Debug.LogError("Problem assessing skill " + thisSkill.GetSkillName() + ". Buffing error.");
                break;
        }
    }

    private void ActivateSkillEffects(GameObject thisSkillParticleEffect, AudioClip thisSkillAudio)
    {
        skillAudioSource.PlayOneShot(thisSkillAudio);
        GameObject thisSkillPE = Instantiate(thisSkillParticleEffect, skillEffectObject.transform.position, Quaternion.identity);

        thisSkillPE.transform.parent = skillEffectObject.transform;
        Destroy(thisSkillPE, 5.0f);
    }
}
