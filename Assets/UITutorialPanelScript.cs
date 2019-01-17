using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialPanelScript : MonoBehaviour {

    [SerializeField] GameObject backButton;
    [SerializeField] List<GameObject> tutorialPanelList;

	// Use this for initialization
	void Start ()
    {
        backButton.SetActive(false);
        DisableAllPanels();
    }

    private void DisableAllPanels()
    {
        foreach (GameObject tutorialPanel in tutorialPanelList)
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void OnClickShowTutorial(int panelNumber)
    {
        DisableAllPanels();
        backButton.SetActive(true);
        tutorialPanelList[panelNumber].SetActive(true);
    }

    public void OnClickReturnToTutorialMainMenu()
    {
        DisableAllPanels();
        backButton.SetActive(false);
    }
}
