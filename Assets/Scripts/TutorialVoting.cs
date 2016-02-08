using UnityEngine;
using System.Collections;

public class TutorialVoting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameData.RoundNo != 1)
        {
            gameObject.SetActive(false);
        }
    }
}
