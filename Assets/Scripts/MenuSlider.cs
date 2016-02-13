using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour {
    Text _text;
    string _init_text;
    Slider _slider;
    Text _label_text;

    // Use this for initialization
    void Start () {
        _text = this.GetComponent<Text>();
        _label_text = transform.FindChild("Label").GetComponent<Text>();
        _init_text = _label_text.text;
        _slider = transform.FindChild("Slider").GetComponent<Slider>();
        _slider.value = PlayerPrefs.GetFloat(this.name, _slider.value);
        UpdateLabel();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateLabel()
    {
        int value = (int)_slider.value;
        if (this.name == "PlayersCount")
        {
            Settings.PlayerCount = (int)value;
            _label_text.text = _init_text + ": " + Settings.PlayerCount;
        }
        else if (this.name == "RoundsCount")
        {
            Settings.RoundCount = (int)value;
            _label_text.text = _init_text + ": " + Settings.RoundCount;
        }
        else if (this.name == "TimeCount")
        {
            Settings.SecondsCount = (int)value;
            _label_text.text = _init_text + ": " + Settings.SecondsCount;
        }
        else
        {
            throw new System.Exception("Trying to update an unknown label " + this.name);
        }
        PlayerPrefs.SetFloat(this.name, _slider.value);
    }
}
