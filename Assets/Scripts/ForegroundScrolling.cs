using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundScrolling : MonoBehaviour {

    [SerializeField] float scrollingSpeed = 1f;

    Vector3 rescrollingPosVector;
    RectTransform myFGRectTransform;

	// Use this for initialization
	void Start ()
    {
        myFGRectTransform = GetComponent<RectTransform>();
        rescrollingPosVector = new Vector3(1820f, myFGRectTransform.localPosition.y, myFGRectTransform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update ()
    {
        myFGRectTransform.localPosition += Vector3.left * scrollingSpeed;

        if (myFGRectTransform.localPosition.x <= -2900f)
        {
            myFGRectTransform.localPosition = rescrollingPosVector;
        }
	}
}
