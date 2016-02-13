using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrossComp : MonoBehaviour {
    Image _crossImage;

	// Use this for initialization
	void Start () {
        _crossImage = GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
        _crossImage.enabled = Settings.PlayerCount == 2;
	}
}
