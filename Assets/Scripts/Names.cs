using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Names : MonoBehaviour
{
    public GameObject _button;
    public GameObject _requirement;
    public GameObject _nameInput;
    public GameObject _namesList;
    public GameObject _confirmPanel;
    InputField _inputField;
    Text _req_text;

    UnityEngine.Object _name_prefab;

    readonly int MAX_LENGTH = 20;

    Regex name_regex = new Regex("^[a-zA-Z0-9 ]*$");

    void Awake()
    {
        _name_prefab = Resources.Load("Name");
    }

    void Start()
    {
        List<string> names = new List<string>();
        names.InsertRange(0, Settings.names_list);
        if (PlayerPrefs.HasKey(Settings._names_key))
        {
            names.InsertRange(0, PlayerPrefs.GetString(Settings._names_key).Split(':'));
        }
        names.Sort();
        foreach (string name in names)
        {
            // Skip the used ones
            if (!GameData.names.Contains(name))
            {
                GameObject new_name_obj = Instantiate(_name_prefab) as GameObject;
                new_name_obj.name = name;
                new_name_obj.transform.FindChild("Name Text").gameObject.GetComponent<Text>().text = name;

                new_name_obj.transform.SetParent(_namesList.transform, false);
                new_name_obj.SetActive(true);
            }
        }

        _inputField = _nameInput.transform.GetComponent<InputField>();
        _req_text = _requirement.transform.Find("Text").GetComponent<Text>();
    }

    void Update()
    {
        if (_confirmPanel.activeSelf)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _confirmPanel.SetActive(true);
        }

        bool name_correct = name_regex.IsMatch(_inputField.text) && _inputField.text.Length != 0 && _inputField.text.Length <= MAX_LENGTH;

        _requirement.SetActive(!name_correct);
        _req_text.text = _inputField.text.Length == 0 ? "pick or write a name" : (_inputField.text.Length > MAX_LENGTH ? "the name is too long" : "forbidden symbols in the name");
        _button.SetActive(name_correct);
    }

    public void NameSelected(string new_name)
    {
        _inputField.text = new_name;
    }

    public void Button()
    {
        string new_name = _inputField.text;

        if (PlayerPrefs.HasKey(Settings._names_key))
        {
            // Add if does not exists yet
            string[] new_names = PlayerPrefs.GetString(Settings._names_key).Split(':');
            if (new_names.Count(p => p == new_name) == 0 && Settings.names_list.Count(p => p == new_name) == 0)
            {
                PlayerPrefs.SetString(Settings._names_key, PlayerPrefs.GetString(Settings._names_key) + ":" + _inputField.text);
            }
        }
        else
        {
            PlayerPrefs.SetString(Settings._names_key, _inputField.text);
        }
        PlayerPrefs.Save();

        GameData.names[GameData.PlayerNo] = new_name;
        GameData.Next();
    }
}
