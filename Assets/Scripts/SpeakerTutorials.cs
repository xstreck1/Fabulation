using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SpeakerTutorials : MonoBehaviour
{
    public GameObject _suggestionScreen;
    public GameObject _buttonScreen;
    public GameObject _progressScreen;

    private string[] suggestions_texts_simple =
    {
        "", " once had a big trouble",
        "", " was the cause of the trouble",
        " however, because of ", " there was no reason for panic ",
        " and the situation was saved with a handy ", "",
        " which called ", " for an assistance ",
        " and thus the day would be saved, were it not for", "",
        " who brought down a mighty ", "",
        " the resulting ", " was unsettlign to say the least",
        " and the heroic ", " had to be called to deal with that"
    };

    private string[] suggestions_texts_hard =
   {
        " once had a big trouble ", " with ",
        " the cause of the trouble ", " was ",
        " ", " there was no reason for panic because of ",
        " the situation was saved with a handy ", "",
        " called ", " for an assistance ",
        " the day would be saved, were it not for", "",
        " brought down a mighty ", "",
        " the resulting ", " was unsettlign to say the least",
        " and the heroic ", " had to be called to deal with that"
    };

    // Use this for initialization
    void Start()
    {
        if (!Settings.Tutorial || GameData.RoundNo != 0)
        {
            _suggestionScreen.SetActive(false);
            _buttonScreen.SetActive(false);
            _progressScreen.SetActive(false);
        }
        else
        {
            _suggestionScreen.SetActive(true);
            string[] suggestions_texts = Settings.HardMode ? suggestions_texts_hard : suggestions_texts_simple;
            string suggestion = GameData.history[GameData.history.Count - 1].text;
            string[] parts = suggestion.Split(new string[] { "..." }, StringSplitOptions.None);
            suggestion = parts[0] + suggestions_texts[GameData.PlayerNo * 2] + parts[1] + suggestions_texts[GameData.PlayerNo * 2 + 1] + parts[2];

            _suggestionScreen.transform.FindChild("Text").gameObject.GetComponent<Text>().text += suggestion;
            _suggestionScreen.transform.FindChild("Text").FindChild("Image").FindChild("Text").GetComponent<Text>().text = _suggestionScreen.transform.FindChild("Text").gameObject.GetComponent<Text>().text;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseOnTouch()
    {
        if (_suggestionScreen.activeSelf)
        {
            _suggestionScreen.SetActive(false);
            _buttonScreen.SetActive(true);
        }
        else if (_buttonScreen.activeSelf)
        {

            _buttonScreen.SetActive(false);
            _progressScreen.SetActive(true);
        }
        else
        {
            _progressScreen.SetActive(false);
        }
    }

    public bool TutorialActive()
    {
        return _suggestionScreen.activeSelf || _buttonScreen.activeSelf || _progressScreen.activeSelf;
    }
}
