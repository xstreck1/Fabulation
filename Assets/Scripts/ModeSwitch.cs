using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour {
    Text _button_text;
    public GameObject livesCount;
    public GameObject pointsCount;

    // Read from the player prefs whether the dictionary has been chosen
    void Start()
    {
        _button_text = transform.FindChild("Text").GetComponent<Text>();
        StaticData.current_mode_ID = PlayerPrefs.GetInt(this.name, 0);
        SetValue();
    }

    public void Click()
    {
        StaticData.current_mode_ID = (StaticData.current_mode_ID + 1) % StaticData.mode_list.Count;
        PlayerPrefs.SetInt(this.name, StaticData.current_mode_ID);
        SetValue();
        bool using_lives = StaticData.mode_list[StaticData.current_mode_ID].using_lives;
        livesCount.SetActive(using_lives);
        pointsCount.SetActive(!using_lives);
    }

    // 
    void SetValue()
    {
        _button_text.text = StaticData.mode_list[StaticData.current_mode_ID].name;
    }
}
