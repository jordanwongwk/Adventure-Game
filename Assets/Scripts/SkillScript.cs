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

        // If its player
        if (GetComponent<EnemyScript>() != null)
        {
            SetupEnemySkills();
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

                        // TODO charIsUsingSkill = true;
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        SkillType thisSkillType = thisCharacterSkills[i].GetSkillType();

                        switch (thisSkillType)
                        {
                            case SkillType.HealthPointRelated:
                                ProcessHealthPointRelatedSkill(thisCharacterSkills[i]);
                                break;

                            case SkillType.Buffing:
                                ProcessBuffingSkill(thisCharacterSkills[i]);
                                break;

                            default:
                                Debug.LogError("Error in processing " + thisCharacterSkills[i].GetSkillName() + " at " + currentTime.ToString());
                                break;
                        }
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

                        charIsUsingSkill = true;
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        SkillType thisSkillType = thisCharacterSkills[i].GetSkillType();

                        switch (thisSkillType)
                        {
                            case SkillType.HealthPointRelated:
                                ProcessHealthPointRelatedSkill(thisCharacterSkills[i]);
                                break;

                            case SkillType.Buffing:
                                ProcessBuffingSkill(thisCharacterSkills[i]);
                                break;

                            default:
                                Debug.LogError("Error in processing " + thisCharacterSkills[i].GetSkillName() + " at " + currentTime.ToString());
                                break;
                        }
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

                        // TODO charIsUsingSkill = true;
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        SkillType thisSkillType = thisCharacterSkills[i].GetSkillType();

                        switch (thisSkillType)
                        {
                            case SkillType.HealthPointRelated:
                                ProcessHealthPointRelatedSkill(thisCharacterSkills[i]);
                                break;

                            case SkillType.Buffing:
                                ProcessBuffingSkill(thisCharacterSkills[i]);
                                break;

                            default:
                                Debug.LogError("Error in processing " + thisCharacterSkills[i].GetSkillName() + " at " + currentTime.ToString());
                                break;
                        }
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

                        // TODO charIsUsingSkill = true;
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        SkillType thisSkillType = thisCharacterSkills[i].GetSkillType();

                        switch (thisSkillType)
                        {
                            case SkillType.HealthPointRelated:
                                ProcessHealthPointRelatedSkill(thisCharacterSkills[i]);
                                break;

                            case SkillType.Buffing:
                                ProcessBuffingSkill(thisCharacterSkills[i]);
                                break;

                            default:
                                Debug.LogError("Error in processing " + thisCharacterSkills[i].GetSkillName() + " at " + currentTime.ToString());
                                break;
                        }
                        break;
                    }
                }
                break;

            default:
                Debug.LogError("Error in processing skill.");
                break;
        }
    }



    private void ProcessHealthPointRelatedSkill(SkillInfo thisSkill)
    {
        float totalDamage = (thisSkill as HealthPointSkills).GetDamageMultiplier() * thisCharacter.GetThisCharStrength();
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
                myCombatUIManager.DisplaySkillEffectText(thisCharacter.name + " uses " + thisSkill.GetSkillName() + "\nDealing " + (int)totalDamage +
                                                        " to " + thisCharacterOpponent.name + "!");
                break;

            default:
                Debug.LogError("Can't log target for skill " + thisSkill.GetSkillName());
                break;
        }
    }

    private void ProcessBuffingSkill(SkillInfo thisSkill)
    {
        // Get Target
        // Get Target Character
        // Buff STR
        // Buff DEF
        // Buff SPD
    }
}
