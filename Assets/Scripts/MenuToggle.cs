using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuToggle : MonoBehaviour {
    Toggle _toggle;


	// Read from the player prefs whether the dictionary has been chosen
	void Start () {
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(_toggle.name, Convert.ToInt32(_toggle.isOn)));
        SetDictionary();
    }
	
    // Add or remove the dictionary from the list in static data
    public void SetDictionary()
    {
        if (StaticData.lists.Any(str => str.Contains(name)))
        {
            if (!_toggle.isOn)
            {
                StaticData.lists.Remove(name);
            }
        }
        else
        {
            if (_toggle.isOn)
            {
                StaticData.lists.Add(name);
            }
        }
        PlayerPrefs.SetInt(_toggle.name, Convert.ToInt32(_toggle.isOn));
    }
}
