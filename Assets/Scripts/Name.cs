using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Name : MonoBehaviour {
    Names _namesPanel;
    
	// Use this for initialization
	void Start () {
        _namesPanel = GameObject.Find("Canvas").GetComponent<Names>();
        GetComponent<Button>().onClick.AddListener(() => ChangeName());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeName()
    {
        _namesPanel.GetComponent<Names>().NameSelected(name);
    }
}
