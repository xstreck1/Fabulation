using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuToggle : MonoBehaviour {
    Toggle _toggle;


	// Use this for initialization
	void Start () {
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(_toggle.name, Convert.ToInt32(_toggle.isOn)));
        SetDictionary();
    }
	
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
