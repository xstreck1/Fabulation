using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour {
    public GameObject _button_text;
    public GameObject _label_text;

    // Read from the player prefs whether the dictionary has been chosen
    void Start()
    {
        SetValue();
    }

    public void Click()
    {
        Settings.simple = !Settings.simple;
        SetValue();
    }

    // 
    void SetValue()
    {
        _label_text.GetComponent<Text>().text = Settings.simple ? "simple mode" : "advanced mode";
        _button_text.GetComponent<Text>().text = Settings.simple ? "switch to advanced" : "switch to simple";
    }
}
