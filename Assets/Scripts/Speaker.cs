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
    public GameObject _storyTitle;
    public GameObject _confirmPanel;
    public GameObject _suggestionScreen;
    public GameObject _tutorialScreen;

    const string title_text = Settings.LANGUAGE == "CZ" ? "Pribeh o {0}" : "Story of {0}";
    const string conclusion_text = Settings.LANGUAGE == "CZ" ? ", ... Konec." : ", ... The End.";

    bool TutorialActive { get { return _suggestionScreen.activeSelf || _tutorialScreen.activeSelf;  } }

#if UNITY_EDITOR
    readonly float BUTTON_BLOCK = 0.1f; // A timeout for the button not to be pressed hastily 
#else
    readonly float BUTTON_BLOCK = 1.5f;
#endif

    public float Timer { get; private set; }

    void Awake()
    {
        SetText();
        SetTitle();
        SetPageNumber();
    }

    void Start()
    {
        Timer = Settings.Seconds;
    }


    void Update()
    {
        if (_confirmPanel.activeSelf)
        {
            return;
        }
         else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _confirmPanel.SetActive(true);
        }
        else if (TutorialActive)
        {
            return;
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
        string the_text;
        if (GameData.FirstPlayer) // First word
        {
            string story_name = GameData.words.GetWord(true);
            GameData.title = String.Format(title_text, story_name);
            if (Settings.HardMode)
            {
                the_text = story_name + "...\n" + GameData.words.GetWord(true) + "...";
                
            }
            else
            {
                the_text = "..." + story_name + "...";
            }
        }
        else if (GameData.LastPlayer) // Last round, last player.
        {
            if (Settings.HardMode)
            {
                the_text = ", " + GameData.words.GetConnective() + "...\n" + conclusion_text;
            }
            else
            {
                the_text = conclusion_text;
            }

        }
        else
        {
            if (Settings.HardMode)
            {
                the_text = ", " + GameData.words.GetConnective() + "...\n" + GameData.words.GetWord(true) + "...";
            }
            else
            {
                the_text = ", ..." + GameData.words.GetWord(true) + "...";
            }
        }

        _newWord.GetComponent<Text>().text = the_text;
        _usedWords.GetComponent<Text>().text = GameData.GetStoryText();

        GameData.history.Add(new UsedWord() { round = GameData.RoundNo, player = GameData.PlayerNo, text = the_text });
    }


    void SetPageNumber()
    {
        _pageNumber.GetComponent<Text>().text = (GameData.RoundNo * Settings.players + GameData.PlayerNo + 1) + "/" + (Settings.players * Settings.rounds);
    }

    void SetTitle()
    {
        _storyTitle.GetComponent<Text>().text = GameData.title;
    }

    public void Check()
    {
        if (Settings.Seconds - Timer > BUTTON_BLOCK)
        {
            GameData.Next();
        }
    }
}
