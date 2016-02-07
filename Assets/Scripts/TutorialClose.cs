using UnityEngine;
using System.Collections;

public class TutorialClose : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if (!Settings.Tutorial)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseOnTouch()
    {
        gameObject.SetActive(false);
    }
}
