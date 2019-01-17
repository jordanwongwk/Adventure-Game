using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextChangerScript : MonoBehaviour {

    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;
    [SerializeField] Text pageNumber;
    [SerializeField] List<GameObject> textGOLists;

    int currentPage = 0;

	// Use this for initialization
	void Start ()
    {
        ChangePage();
    }

    private void ChangePage()
    {
        foreach (GameObject text in textGOLists)
        {
            text.SetActive(false);
        }

        textGOLists[currentPage].SetActive(true);

        pageNumber.text = (currentPage + 1).ToString() + " / " + textGOLists.Count.ToString();
        CheckForButtonsInteractable();
    }

    private void CheckForButtonsInteractable()
    {
        if (currentPage == 0) { previousButton.interactable = false; }
        else { previousButton.interactable = true; }

        if (currentPage >= textGOLists.Count - 1) { nextButton.interactable = false; }
        else { nextButton.interactable = true; }
    }

    public void OnClickChangeTextToNext()
    {
        currentPage++;
        ChangePage();
    }


    public void OnClickChangeTextToPrevious()
    {
        currentPage--;
        ChangePage();
    }
}
