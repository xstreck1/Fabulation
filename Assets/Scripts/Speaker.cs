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
        // Timing related progress
        float old_timer = Timer;
        Timer -= Time.deltaTime;

         if (old_timer > 0 && Timer <= 0)
        {
            Check();
        }

        // Game control
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }
   
    void SetText()
    {
        if (GameData.FirstPlayer) // First word
        {
            _newWord.GetComponent<Text>().text = "The Story How a \n" + GameData.words.GetWord(true) + "...";
        }
        else if (GameData.LastPlayer)
        { // Last round, last player.
            _newWord.GetComponent<Text>().text = ", ... The End.";
        }
        else
        {
            if (Settings.simple)
            {
                _newWord.GetComponent<Text>().text = ", ..." + GameData.words.GetWord(true) + "...";
            }
            else
            {
                _newWord.GetComponent<Text>().text = ", " + GameData.words.GetConnective() + "...\n" + GameData.words.GetWord(true) + "...";
            }
        }
        _usedWords.GetComponent<Text>().text = GetStoryText();
    }


    void SetPageNumber()
    {
        _pageNumber.GetComponent<Text>().text = (GameData.RoundNo * Settings.players + GameData.PlayerNo + 1) + "/" + (Settings.players * Settings.rounds);
    }

    string GetStoryText()
    {
        string text = "";
        foreach (UsedWord word in GameData.history)
        {
            text += word.text;
        }
        return text.Replace("\n", " ");
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
            GameData.history.Add(new UsedWord() { round = GameData.RoundNo, player = GameData.PlayerNo, text = _newWord.GetComponent<Text>().text });
            GameData.Next();
        }
    }
}
