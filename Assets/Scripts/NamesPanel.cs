using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NamesPanel : MonoBehaviour {
    public GameObject _namesList;
    Text _instructionsText;
    Game _game;
    
    Object _name_prefab;
    public string SelectedName 
    {
        get; private set;
    }

    void Awake()
    {
        _game = GameObject.Find("Canvas").GetComponent<Game>();
        _instructionsText = transform.Find("Instructions/Instructions Text").GetComponent<Text>();
        _name_prefab = Resources.Load("Name");
    }

    // Use this for initialization
    void Start ()
    {
        foreach (string name in StaticData.names_list)
        {
            GameObject new_name_obj = Instantiate(_name_prefab) as GameObject;
            new_name_obj.name = name;
            new_name_obj.GetComponent<Text>().text = name;
            new_name_obj.transform.parent = _namesList.transform;
            new_name_obj.SetActive(true);
        }
    }

    void OnEnable()
    {
        NameSelected("");
    }

    public void NameSelected(string new_name) {
        SelectedName = new_name;
        _instructionsText.text = GlobalMethods.ReplaceName(_instructionsText.text, new_name);
    }

    public void PlayClick()
    {
        if (SelectedName != "")
        {
            StaticData.names[_game.PlayerNo] = SelectedName;
            _namesList.transform.FindChild(SelectedName).gameObject.SetActive(false); // Disable the name for the next user
            _game.Next();
        }
    }
}
