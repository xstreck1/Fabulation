using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Name : MonoBehaviour {
    NamesPanel _namesPanel;
    
	// Use this for initialization
	void Start () {
        _namesPanel = GameObject.Find("Names Panel").GetComponent<NamesPanel>();
        GetComponent<Button>().onClick.AddListener(() => ChangeName());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ChangeName()
    {
        _namesPanel.GetComponent<NamesPanel>().NameSelected(name);
    }
}
