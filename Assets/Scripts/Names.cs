using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Names : MonoBehaviour {
    public GameObject _button;
    public GameObject _requirement;
    public GameObject _namesList;
    public GameObject _nameInput;

    Text _name_input_text;
    
    Object _name_prefab;
    bool _own_name = false;

    public string SelectedName 
    {
        get; private set;
    }

    void Awake()
    {
        _name_prefab = Resources.Load("Name");
    }
    
    void Start ()
    {
        foreach (string name in Settings.names_list)
        {
            // Skip the used ones
            if (!GameData.names.Contains(name))
            {
                GameObject new_name_obj = Instantiate(_name_prefab) as GameObject;
                new_name_obj.name = name;
                new_name_obj.transform.FindChild("Name Text").gameObject.GetComponent<Text>().text = name;
#if UNITY_ANDROID && !UNITY_EDITOR
            new_name_obj.transform.localScale = Vector3.one * 2;
#endif
                new_name_obj.transform.SetParent(_namesList.transform);
                new_name_obj.SetActive(true);
            }
        }
        _name_input_text = _nameInput.transform.FindChild("Text").GetComponent<Text>();
    }

    void Update()
    {
        if (_own_name)
        {
            bool has_name = _name_input_text.text != "";
            _requirement.SetActive(!has_name);
            _button.SetActive(has_name);
        }
    }

    public void NameSelected(string new_name)
    {
        GameData.names[GameData.PlayerNo] = SelectedName;
        GameData.Next();
    }

    public void Button()
    {
        if (!_own_name)
        {
            _nameInput.SetActive(true);
            _namesList.SetActive(false);
            _requirement.SetActive(true);
            _button.SetActive(false);
            _button.transform.FindChild("Text").GetComponent<Text>().text = "that is my name";
            _own_name = true;
        }
        else
        {
            GameData.names[GameData.PlayerNo] = _name_input_text.text;
            GameData.Next();
        }
    }
}
