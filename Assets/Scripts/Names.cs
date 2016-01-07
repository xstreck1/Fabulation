using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Names : MonoBehaviour
{
    public GameObject _button;
    public GameObject _namesList;
    public GameObject _confirmPanel;
    public GameObject _instructionsText;


    Text _name_input_text;

    Object _name_prefab;
    bool _own_name = false;

    void Awake()
    {
        _name_prefab = Resources.Load("Name");
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
#if UNITY_ANDROID && !UNITY_EDITOR
            new_name_obj.transform.localScale = Vector3.one * 2;
#endif
                new_name_obj.transform.SetParent(_namesList.transform);
                new_name_obj.SetActive(true);
            }
        }
    }

    public void NameSelected(string new_name)
    {
        GameData.names[GameData.PlayerNo] = new_name;
        GameData.Next();
    }

    public void Button()
    {
        Application.LoadLevel("NewName");
    }
}
