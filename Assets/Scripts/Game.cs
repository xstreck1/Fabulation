using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public struct UsedWord
{
    public int round;
    public int player;
    public string text;
    public bool accepted;
}

public class Game : MonoBehaviour
{
    public GameObject _namesPanel;
    public GameObject _speakerPanel;
    public GameObject _judgePanel;
    public GameObject _switchPanel;
    public GameObject _instructionsSpeaker;
    public GameObject _instructionsJudge;
    public GameObject _newWord;
    public GameObject _usedWords;
    public GameObject _textPad;
    public GameObject _check;
    public GameObject _timeMeter;
    public GameObject _pageNumber;
    public GameObject _votes;
    public GameObject _speak;

#if UNITY_EDITOR
    readonly float PASS_TIME = 0.1f;
#else
    readonly float PASS_TIME = 1.5f;
#endif
    readonly int PHASE_COUNT = 3; // Three phases per player
    readonly float BUTTON_BLOCK = 2; // A second timeout for the button

    Words _words;
    float _timer;
    int _step_no;

    public int PlayerNo { get { return (_step_no / PHASE_COUNT) % StaticData.players; } }
    public int RoundNo { get { return _step_no / (PHASE_COUNT * StaticData.players); } }
    bool FirstPlayer { get { return _step_no / PHASE_COUNT == 0; } }
    bool LastPlayer { get { return RoundNo + 1 >= StaticData.rounds && PlayerNo == StaticData.players - 1; } }
    int VotesAvailable { get; set; }

    List<UsedWord> _history;
    List<bool> _accepted;
    List<GameObject> _judgers;
    List<int> _last_score;

    void Start()
    {
        StaticData.ResetScore();
        _words = new Words();
        _history = new List<UsedWord>();
        _judgers = new List<GameObject>();
        _accepted = Enumerable.Repeat(false, StaticData.players).ToList();
        _last_score = Enumerable.Repeat(0, StaticData.players).ToList();
        StaticData.names = Enumerable.Repeat<Func<string>>(_words.GetName, StaticData.players).Select(f => f()).ToList();

        SetPlayerName();

        _namesPanel.SetActive(false);
        _judgePanel.SetActive(false);
        _switchPanel.SetActive(false);
        _speakerPanel.SetActive(false);

        CreateJudgeButtons();
        SetPanel();
    }


    void Update()
    {
        // Timing related progress
        double old_timer = _timer;
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            float progress = (StaticData.Seconds - _timer) / StaticData.Seconds;
            _timeMeter.transform.localScale = new Vector3(progress, 1f, 1f);
        }
        else if (old_timer > 0 && _timer <= 0)
        {
            if (((_step_no % 3) == 2))
            {
                Next();
            }
            else if (((_step_no % 3) == 1))
            {
                Check();
            }
        }
        else
        {
            _timeMeter.transform.localScale = Vector3.one;
        }

        // Game control
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }

    void CreateJudgeButtons()
    {
        var prefab = Resources.Load("Judger");
        foreach (int i in Enumerable.Range(0, StaticData.players - 1))
        {
            if (i > 9)
            {
                // To fix this, change the way the judger obtain its ID (currently from its Name)
                throw new System.IndexOutOfRangeException("Max 10 players allowed. More would break the code.");
            }
            GameObject new_judger = Instantiate(prefab) as GameObject;
            new_judger.name = "Judger" + (i);
            new_judger.transform.position = new Vector3(0, -100 - 80 * i, 0);
            new_judger.transform.SetParent(_judgePanel.transform, false);
            new_judger.GetComponent<Toggle>().onValueChanged.AddListener((value) => JudgeClick(value));
            _judgers.Add(new_judger);
        }
    }

    void SetPanel()
    {
        switch (_step_no % 3)
        {
            case 0:
                _timer = 0;
                _switchPanel.SetActive(false);
                if (RoundNo == 0)
                {
                    _namesPanel.SetActive(true);
                    SetNamesList();
                }
                else
                {
                    _judgePanel.SetActive(true);
                    SetPlayerName();
                    SetJudgeControls();
                }
                break;
            case 1:
                _namesPanel.SetActive(false);
                _judgePanel.SetActive(false);
                _speakerPanel.SetActive(true);

                _timer = !FirstPlayer && !LastPlayer ? StaticData.Seconds : 0;
                SetPlayerName();
                SetText();
                break;
            case 2:
                _speakerPanel.SetActive(false);
                _switchPanel.SetActive(true);

                _timer = PASS_TIME;
                break;
        }
    }

    void SetNamesList()
    {

    }

    void SetJudgeControls()
    {
        int tested_round = _step_no / PHASE_COUNT - StaticData.players + 1;
        int active_count = 0;
        foreach (GameObject judger in _judgers)
        {
            Text current_text = judger.transform.FindChild("Word").FindChild("Text").GetComponent<Text>();
            judger.SetActive(true);
            bool to_judge = _history[tested_round].accepted;
            if (to_judge)
            {
                active_count++;
                current_text.text = _history[tested_round].text;
            }
            else
            {
                current_text.text = "skipped";
            }
            tested_round++;
        }
        VotesAvailable = Mathf.Min(active_count, StaticData.players / 2);
        SetVoteNumbers(VotesAvailable);
    }

    void SetVoteNumbers(int remaining)
    {
        // TODO set bottom
        if (remaining > 0)
        {
            _speak.SetActive(false);
            _votes.SetActive(true);
            string vote_word = remaining == 1 ? " VOTE " : " VOTES ";
            _votes.GetComponent<Text>().text = remaining.ToString() + vote_word + "REMAINING";
        }
        else
        {
            _speak.SetActive(true);
            _votes.SetActive(false);
        }
    }

    void SetText()
    {
        if (FirstPlayer) // First word
        {
            _newWord.GetComponent<Text>().text = "The story how\n" + _words.GetWord(true) + "...";
        }
        else if (LastPlayer)
        { // Last round, last player.
            _newWord.GetComponent<Text>().text = ", ... The End.";
        }
        else
        {
            if (StaticData.simple)
            {
                _newWord.GetComponent<Text>().text = ", ..." + _words.GetWord(true) + "...";
            }
            else
            {
                _newWord.GetComponent<Text>().text = ", " + _words.GetConnective() + "...\n" + _words.GetWord(true) + "...";
            }
        }
        _usedWords.GetComponent<Text>().text = GetStoryText();
    }

    public void Next()
    {
        _step_no++;
        if (RoundNo >= StaticData.rounds)
        {
            Finish();
        }
        else
        {
            SetPanel();
        }
        SetPageNumber();
    }

    void SetPageNumber()
    {
        _pageNumber.GetComponent<Text>().text = (RoundNo * StaticData.players + PlayerNo + 1) + "/" + (StaticData.players * StaticData.rounds);
    }

    void Finished(bool is_accepted)
    {
        _history.Add(new UsedWord() { round = _step_no, player = PlayerNo, text = _newWord.GetComponent<Text>().text, accepted = is_accepted });
    }

    string GetStoryText()
    {
        string text = "";
        foreach (UsedWord word in _history)
        {
            if (word.accepted)
            {
                text += word.text;
            }
        }
        return text.Replace("\n", " ");
    }

    void SetPlayerName()
    {
        string currentName = StaticData.names[PlayerNo];
        _instructionsSpeaker.GetComponent<Text>().text = GlobalMethods.ReplaceName(_instructionsSpeaker.GetComponent<Text>().text, currentName);
        _instructionsJudge.GetComponent<Text>().text = GlobalMethods.ReplaceName(_instructionsJudge.GetComponent<Text>().text, currentName);
    }

    public void Check()
    {
        if (StaticData.Seconds - _timer > BUTTON_BLOCK)
        {
            Finished(true);
            Next();
        }
    }


    public void Speak()
    {
        for (int i = 0; i < _judgers.Count; i++)
        {
            // The player that spoke the earliest is the successor of the curent player
            int affected_player = (PlayerNo + i + 1) % StaticData.players;
            // TODO count score
            if (_judgers[i].transform.FindChild("Word").FindChild("Text").GetComponent<Text>().text != "skipped")
            {
                Toggle toggle = _judgers[i].GetComponent<Toggle>();
                _last_score[affected_player] += toggle.isOn ? 1 : -1;
                StaticData.score[affected_player] += toggle.isOn ? 1 : -1;
                toggle.enabled = true;
                toggle.isOn = false;
            }
        }
        Next();
    }

    public void JudgeClick(bool on)
    {
        int remaining = VotesAvailable - _judgers.Sum(j => j.GetComponent<Toggle>().isOn ? 1 : 0);
        if (on && remaining == 0)
        {
            _judgers.ForEach(j => j.GetComponent<Toggle>().enabled = j.GetComponent<Toggle>().isOn); // Disable those not on
        }
        else if (!on && remaining == 1)
        {
            _judgers.ForEach(j => j.GetComponent<Toggle>().enabled = true);
        }
        SetVoteNumbers(remaining);
    }

    public void Finish()
    {
        StaticData.story = GetStoryText();
        Application.LoadLevel("Score");
    }
}
