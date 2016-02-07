using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class Speaker : MonoBehaviour
{
    public GameObject _instructionsText;
    public GameObject _newWord;
    public GameObject _usedWords;
    public GameObject _textPad;
    public GameObject _pageNumber;
    public GameObject _confirmPanel;


    const string intro_text = Settings.LANGUAGE == "CZ" ? "Pribeh o {0}" : "Story of {0}";
    const string conclusion_text = Settings.LANGUAGE == "CZ" ? ", ... Konec." : ", ... The End.";

#if UNITY_EDITOR
    readonly float BUTTON_BLOCK = 0.1f; // A timeout for the button not to be pressed hastily 
#else
    readonly float BUTTON_BLOCK = 1.5f;
#endif

    public float Timer { get; private set; }

    void Start()
    {
        SetPlayerName();
        SetText();
        SetPageNumber();
        Timer = Settings.Seconds;
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

        // Timing related progress
        float old_timer = Timer;
        Timer -= Time.deltaTime;

        if (old_timer > 0 && Timer <= 0)
        {
            Check();
        }
    }
   
    void SetText()
    {
        string text_to_save = "";
        if (GameData.FirstPlayer) // First word
        {
            string story_name = GameData.words.GetWord(true);
            text_to_save = "..." + story_name + "...";
            GameData.title = String.Format(intro_text, story_name);
            _newWord.GetComponent<Text>().text = GameData.title  + "\n" + text_to_save;
        }
        else if (GameData.LastPlayer)
        { // Last round, last player.
            text_to_save = conclusion_text;
           _newWord.GetComponent<Text>().text = text_to_save;

        }
        else
        {
            if (Settings.HardMode)
            {
                text_to_save = ", " + GameData.words.GetConnective() + "...\n" + GameData.words.GetWord(true) + "...";
                _newWord.GetComponent<Text>().text = text_to_save;
            }
            else
            {
                text_to_save = ", ..." + GameData.words.GetWord(true) + "...";
                _newWord.GetComponent<Text>().text = text_to_save;
            }
        }
        _usedWords.GetComponent<Text>().text = GameData.title + "\n" + GameData.GetStoryText();

        GameData.history.Add(new UsedWord() { round = GameData.RoundNo, player = GameData.PlayerNo, text = text_to_save });
    }


    void SetPageNumber()
    {
        _pageNumber.GetComponent<Text>().text = (GameData.RoundNo * Settings.players + GameData.PlayerNo + 1) + "/" + (Settings.players * Settings.rounds);
    }

    void SetPlayerName()
    {
        string currentName = GameData.names[GameData.PlayerNo];
        _instructionsText.GetComponent<Text>().text = GlobalMethods.ReplaceName(_instructionsText.GetComponent<Text>().text, currentName);
    }

    public void Check()
    {
        if (Settings.Seconds - Timer > BUTTON_BLOCK)
        {
            GameData.Next();
        }
    }
}
