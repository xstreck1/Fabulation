using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SuggestionScreen : MonoBehaviour
{
    public GameObject tutorialScreen;

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
            gameObject.SetActive(false);
        }
        else
        {
            string[] suggestions_texts = Settings.HardMode ? suggestions_texts_hard : suggestions_texts_simple;
            string suggestion = GameData.history[GameData.history.Count - 1].text;
            string[] parts = suggestion.Split(new string[] { "..." }, StringSplitOptions.None);
            suggestion = parts[0] + suggestions_texts[GameData.PlayerNo * 2] + parts[1] + suggestions_texts[GameData.PlayerNo * 2 + 1] + parts[2];

            transform.FindChild("Text").gameObject.GetComponent<Text>().text += suggestion;
            transform.FindChild("Text").FindChild("Image").FindChild("Text").GetComponent<Text>().text = transform.FindChild("Text").gameObject.GetComponent<Text>().text;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseOnTouch()
    {
        tutorialScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
