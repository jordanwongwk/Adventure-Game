using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {

    [SerializeField] LevelManager levelManagerInstance;

	void Awake ()
    {
        levelManagerInstance = LevelManager.instance;
	}

    public void OnClickReturnToMainMenu()
    {
        levelManagerInstance.OnClickReturnToMainMenuScene();
    }
}
