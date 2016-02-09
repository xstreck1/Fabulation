using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuToggle : MonoBehaviour
{
    Toggle _toggle;

    // Read from the player prefs whether the dictionary has been chosen
    void Start()
    {
        transform.FindChild("Label").GetComponent<Text>().text = this.name;
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(_toggle.name, Convert.ToInt32(_toggle.isOn)));
        SetSetting(_toggle.isOn);
        _toggle.onValueChanged.AddListener(SetSetting);
    }

    // 
    public void SetSetting(bool value)
    {
        if (this.name == "Tutorial")
        {
            Settings.Tutorial = value;
        }
        else if (this.name == "Competetive")
        {
            Settings.Competetive = value;
        }
        else if (this.name == "Advanced")
        {
            Settings.HardMode = value;
        }

        Settings.used_lists[_toggle.name] = value;
        PlayerPrefs.SetInt(_toggle.name, Convert.ToInt32(Settings.used_lists[_toggle.name]));
    }
}
