using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour {

    [SerializeField] GameObject tutorialPanels;

    public void OnClickEnableTutorialPanel()
    {
        tutorialPanels.SetActive(true);
    }

    public void OnClickCloseTutorialPanel()
    {
        tutorialPanels.SetActive(false);
    }
}
