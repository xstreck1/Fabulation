using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {
    Text _button_text; 

    // Read from the player prefs whether the dictionary has been chosen
    void Start()
    {
        _button_text = transform.FindChild("Text").GetComponent<Text>();
        _button_text.text = PlayerPrefs.GetString(this.name, _button_text.text);
        SetValue();
    }

    public void Click()
    {
        StaticData.game_mode = (StaticData.GameMode) ((int)++StaticData.game_mode % Enum.GetNames(typeof(StaticData.GameMode)).Length);
        _button_text.text = StaticData.game_mode.ToString();
        SetValue();
    }

    // 
    void SetValue()
    {
        StaticData.game_mode = (StaticData.GameMode) Enum.Parse(typeof(StaticData.GameMode), _button_text.text);
        PlayerPrefs.SetString(this.name, _button_text.text);
    }
}
