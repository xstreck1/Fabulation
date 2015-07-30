﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class GenreToggle : MonoBehaviour {
    Toggle _toggle;


	// Read from the player prefs whether the dictionary has been chosen
	void Start () {
        _toggle = GetComponent<Toggle>();
        SetDictionary(Convert.ToBoolean(PlayerPrefs.GetInt(_toggle.name, Convert.ToInt32(_toggle.isOn))));
        _toggle.onValueChanged.AddListener(SetDictionary);
    }
	
    // Add or remove the dictionary from the list in static data
    public void SetDictionary(bool value)
    {
        StaticData.used_lists[_toggle.name] = value;
        PlayerPrefs.SetInt(_toggle.name, Convert.ToInt32(StaticData.used_lists[_toggle.name]));
    }
}
