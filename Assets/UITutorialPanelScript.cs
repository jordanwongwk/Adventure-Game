using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialPanelScript : MonoBehaviour {

    [SerializeField] GameObject backButton;
    [SerializeField] List<GameObject> tutorialPanelList;

    List<GameObject> activePanels = new List<GameObject>();

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
        backButton.SetActive(true);
        tutorialPanelList[panelNumber].SetActive(true);
        activePanels.Add(tutorialPanelList[panelNumber]);
    }

    public void OnClickReturnToTutorialMainMenu()
    {
        activePanels[activePanels.Count - 1].SetActive(false);
        activePanels.RemoveAt(activePanels.Count - 1);

        if (activePanels.Count == 0)
        {
            backButton.SetActive(false);
        }
    }
}
