using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class GenreToggle : MonoBehaviour {
    Toggle _toggle;

	// Read from the player prefs whether the dictionary has been chosen
	void Start ()
    {
        transform.FindChild("Label").GetComponent<Text>().text = this.name;
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(_toggle.name, Convert.ToInt32(_toggle.isOn)));
        SetDictionary(_toggle.isOn);
        _toggle.onValueChanged.AddListener(SetDictionary);
    }
	
    // Add or remove the dictionary from the list in static data
    public void SetDictionary(bool value)
    {
        Settings.used_lists[_toggle.name] = value;
        PlayerPrefs.SetInt(_toggle.name, Convert.ToInt32(Settings.used_lists[_toggle.name]));
    }
}
