using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;

    [SerializeField] int mainMenuSceneInt = 0;
    [SerializeField] int battleSceneInt = 0;

    // Singleton: Only one LevelManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OnClickReturnToMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneInt);
    }

    public void OnClickStartBattleScene()
    {
        SceneManager.LoadScene(battleSceneInt);
    }
}
