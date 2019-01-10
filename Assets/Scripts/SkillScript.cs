using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationTime { beforeCombat, duringCombatOffense, duringCombatDefense, afterCombat }

public class SkillScript : MonoBehaviour {

    [SerializeField] List<SkillInfo> thisCharacterSkills = new List<SkillInfo>();

    bool charIsUsingSkill = false;

    Character thisCharacter;
    Character thisCharacterOpponent;
    CombatUIManager myCombatUIManager;

    // Use this for initialization
    void Start ()
    {
        myCombatUIManager = FindObjectOfType<CombatUIManager>();
        thisCharacter = GetComponent<Character>();

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
                ProcessStatChangeSkill(thisCharacterSkills[skillNumber], thisSkillType);
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
                thisCharacter.ThisCharacterTakingDamage((int)totalDamage);
                break;

            case Target.Opponent:
                thisCharacter.AttackAnimation();
                thisCharacterOpponent.ThisCharacterTakingDamage((int)totalDamage);
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

        thisCharacter.ThisCharacterHeals(healAmount);
        myCombatUIManager.SetTurnOutcomeText(thisCharacter.name + " uses " + thisSkill.GetSkillName() + "!\nRecovering self for " + healAmount + "hp!");
    }

    private void ProcessStatChangeSkill(SkillInfo thisSkill, SkillType thisSkillType)
    {
        // Get Target
        // Get Target Character
        // Buff STR
        // Buff DEF
        // Buff SPD
        // Debuff STR
        // Debuff DEF
        // Debuff SPD
    }
}
