using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Names : MonoBehaviour {
    public GameObject _namesList;
    public GameObject _speak;
    public GameObject _text;
    Text _instructionsText;
    
    Object _name_prefab;
    public string SelectedName 
    {
        get; private set;
    }

    void Awake()
    {
        _instructionsText = transform.Find("Instructions/Instructions Text").GetComponent<Text>();
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
    }

    void OnEnable()
    {
        NameSelected("");
        _speak.SetActive(false);
        _text.SetActive(true);
    }

    public void NameSelected(string new_name) {
        SelectedName = new_name;
        _instructionsText.text = GlobalMethods.ReplaceName(_instructionsText.text, new_name);
        _speak.SetActive(true);
        _text.SetActive(false);
    }

    public void PlayClick()
    {
        if (SelectedName != "")
        {
            GameData.names[GameData.PlayerNo] = SelectedName;
            GameData.Next();
        }
    }
}
