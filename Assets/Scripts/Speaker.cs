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


    const string intro_text = Settings.LANGUAGE == "CZ" ? "Pribeh o tom jak \n" : "The Story How a \n";
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
        if (GameData.FirstPlayer) // First word
        {
            _newWord.GetComponent<Text>().text = intro_text + GameData.words.GetWord(true) + "...";
        }
        else if (GameData.LastPlayer)
        { // Last round, last player.
            _newWord.GetComponent<Text>().text = conclusion_text;
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
        _usedWords.GetComponent<Text>().text = GameData.GetStoryText();
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
            GameData.history.Add(new UsedWord() { round = GameData.RoundNo, player = GameData.PlayerNo, text = _newWord.GetComponent<Text>().text });
            GameData.Next();
        }
    }
}
