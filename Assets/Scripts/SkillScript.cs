using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationTime { beforeCombat, duringCombat, afterCombat }

public class SkillScript : MonoBehaviour {

    [SerializeField] List<SkillInfo> thisCharacterSkills = new List<SkillInfo>();

    List<bool> skillActivationThisRound = new List<bool>();

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

        SetupSkillActivationBooleanList();
    }

    private void SetupSkillActivationBooleanList()
    {
        // Initialize the skill activation boolean List
        foreach (SkillInfo skill in thisCharacterSkills)
        {
            skillActivationThisRound.Add(false);
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

    public void AttemptToActivateSkill()
    {
        float randomValue = Random.Range(0f, 100f);
        Debug.Log("This round RNG " + randomValue);

        for (int i = 0; i < thisCharacterSkills.Count; i++)
        {
            // If the RNG value is between the min and max of the skill's RNG, check the boolean
            if (thisCharacterSkills[i].GetMinActivationRNG() < randomValue && thisCharacterSkills[i].GetMaxActivationRNG() >= randomValue)
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

                    if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                    // TODO activate skill here?
                    skillActivationThisRound[i] = false;
                }

                break;

            case ActivationTime.duringCombat:
                for (int i = 0; i < skillActivationThisRound.Count; i++)
                {
                    if (skillActivationThisRound[i] == false) { continue; }

                    if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
                    // TODO activate skill here?
                    skillActivationThisRound[i] = false;
                }

                break;

            case ActivationTime.afterCombat:
                for (int i = 0; i < skillActivationThisRound.Count; i++)
                {
                    if (skillActivationThisRound[i] == false) { continue; }

                    if (thisCharacterSkills[i].GetSkillActivationTime() != currentTime) { continue; }

                    Debug.Log("Skill activated in " + currentTime.ToString());
                    myCombatUIManager.UsingSkillDisplayUI(this.gameObject, thisCharacterSkills[i].GetSkillName());
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
