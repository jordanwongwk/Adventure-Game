using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillScript : MonoBehaviour {

    List<SkillEffect> thisEnemySkills = new List<SkillEffect>();

    List<bool> skillActivationThisRound = new List<bool>();

    CombatUIManager myCombatUIManager;
    EnemyScript myEnemyScript;
    EnemyInfo thisEnemyInfo;

    // Use this for initialization
    void Start()
    {
        myCombatUIManager = FindObjectOfType<CombatUIManager>();
        myEnemyScript = GetComponent<EnemyScript>();
        InitializingEnemySkills();
    }

    private void InitializingEnemySkills()
    {
        thisEnemyInfo = myEnemyScript.GetThisEnemyInfo();

        for (int i = 0; i < thisEnemyInfo.GetNumberOfEnemySpecialSkill(); i++)
        {
            thisEnemySkills.Add(thisEnemyInfo.GetEnemySpecialSkill(i));
        }

        // Initialize the skill activation boolean List
        foreach (SkillEffect skill in thisEnemySkills)
        {
            skillActivationThisRound.Add(false);
        }
    }

    public void AttemptToActivateSkill()
    {
        float randomValue = Random.Range(0f, 100f);
        Debug.Log("This round RNG " + randomValue);

        for (int i = 0; i < thisEnemySkills.Count; i++)
        {
            // If the RNG value is between the min and max of the skill's RNG, check the boolean
            if (thisEnemySkills[i].GetMinActivationRNG() < randomValue && thisEnemySkills[i].GetMaxActivationRNG() >= randomValue)
            {
                skillActivationThisRound[i] = true;
            }
        }
    }

    public void AttemptToUseSkill(ActivationTime currentTime)
    {
        switch (currentTime)
        {
            case ActivationTime.beforeCombat:
                for (int i = 0; i < skillActivationThisRound.Count; i++)
                {
                    if (skillActivationThisRound[i] == false) { continue; }

                    if (thisEnemySkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisEnemySkills[i].GetSkillName());
                    // TODO activate skill here?
                    skillActivationThisRound[i] = false;
                }

                break;

            case ActivationTime.duringCombat:
                for (int i = 0; i < skillActivationThisRound.Count; i++)
                {
                    if (skillActivationThisRound[i] == false) { continue; }

                    if (thisEnemySkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisEnemySkills[i].GetSkillName());
                    // TODO activate skill here?
                    skillActivationThisRound[i] = false;
                }

                break;

            case ActivationTime.afterCombat:
                for (int i = 0; i < skillActivationThisRound.Count; i++)
                {
                    if (skillActivationThisRound[i] == false) { continue; }

                    if (thisEnemySkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisEnemySkills[i].GetSkillName());
                    // TODO activate skill here?
                    skillActivationThisRound[i] = false;
                }

                break;

            default:
                Debug.LogError("Error in processing skill.");
                break;
        }
    }
}
