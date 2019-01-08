using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationTime { beforeCombat, duringCombatOffense, duringCombatDefense, afterCombat }

public class SkillScript : MonoBehaviour {

    [SerializeField] List<SkillInfo> thisCharacterSkills = new List<SkillInfo>();

    //List<bool> skillActivationThisRound = new List<bool>();

    CombatUIManager myCombatUIManager;

    // Use this for initialization
    void Start ()
    {
        myCombatUIManager = FindObjectOfType<CombatUIManager>();

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

                        Debug.Log("Skill activated in " + currentTime.ToString());
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());

                        
                        //float damage = (thisCharacterSkills[i] as BuffingSkills).GetStrengthBuffMultiplier();
                        // TODO activate skill here?
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

                        Debug.Log("Skill activated in " + currentTime.ToString());
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        // TODO activate skill here?
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

                        Debug.Log("Skill activated in " + currentTime.ToString());
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        // TODO activate skill here?
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

                        Debug.Log("Skill activated in " + currentTime.ToString());
                        myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                        // TODO activate skill here?
                        break;
                    }
                }
                break;

            default:
                Debug.LogError("Error in processing skill.");
                break;
        }
    }
}
