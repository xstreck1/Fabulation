using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour {
    public GameObject _button_text;

    // Read from the player prefs whether the dictionary has been chosen
    void Start()
    {
        SetValue();
    }

    public void Click()
    {
        StaticData.simple = !StaticData.simple;
        SetValue();
    }

    // 
    void SetValue()
    {
        _button_text.GetComponent<Text>().text = StaticData.simple ? "simple" : "advanced";
    }
}
