using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelHandler : MonoBehaviour {

    LevelManager myLevelManager;
    PlayerPrefsManager myPlayerPrefs;

	// Use this for initialization
	void Start ()
    {
        myLevelManager = FindObjectOfType<LevelManager>();
        myPlayerPrefs = FindObjectOfType<PlayerPrefsManager>();
	}

    public void OnClickStartNormalBattleScene()
    {
        myPlayerPrefs.SetMonsterToBattle(0);
        myLevelManager.OnClickStartBattleScene();
    }

    public void OnClickStartHardBattleScene()
    {
        myPlayerPrefs.SetMonsterToBattle(1);
        myLevelManager.OnClickStartBattleScene();
    }
}
