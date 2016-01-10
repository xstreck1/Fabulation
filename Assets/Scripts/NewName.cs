using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NewName : MonoBehaviour
{
    public GameObject _button;
    public GameObject _requirement;
    public GameObject _nameInput;
    public GameObject _instructionsText;

    readonly string _new_button_caption = "that is my name";
    readonly int MAX_LENGTH = 10;

    Regex name_regex = new Regex("^[a-zA-Z0-9 ]*$");

    Text _name_input_text;
    Text _button_text;

    Object _name_prefab;
    bool _own_name = false;

    public string SelectedName
    {
        get; private set;
    }


    void Start()
    {
        _name_input_text = _nameInput.transform.FindChild("Text").GetComponent<Text>();
        _button_text = _button.transform.FindChild("Text").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Names");
        }

        bool has_name = _name_input_text.text.Length != 0;
        bool name_correct = name_regex.IsMatch(_name_input_text.text) && _name_input_text.text.Length <= MAX_LENGTH;

        _requirement.SetActive(has_name && !name_correct);
        _button.SetActive(name_correct);
        _button_text.text = has_name ? "this is what i choose" : "back to selection";
    }

    public void NameSelected(string new_name)
    {
        GameData.names[GameData.PlayerNo] = SelectedName;
        GameData.Next();
    }

    public void Button()
    {
        if (_name_input_text.text.Length == 0)
        {
            Application.LoadLevel("Names");
        }
        else
        {
            if (PlayerPrefs.HasKey(Settings._names_key))
            {
                PlayerPrefs.SetString(Settings._names_key, PlayerPrefs.GetString(Settings._names_key) + ":" + _name_input_text.text);
            }
            else
            {
                PlayerPrefs.SetString(Settings._names_key, _name_input_text.text);
            }
            PlayerPrefs.Save();
            GameData.names[GameData.PlayerNo] = _name_input_text.text;
            GameData.Next();
        }
    }
}
