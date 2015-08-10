﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public struct UsedWord
{
    public int round;
    public int player;
    public string text;
    public bool accepted;
}

public class Game : MonoBehaviour
{
    public GameObject _narratorPanel;
    public GameObject _judgePanel;
    public GameObject _icon;
    public GameObject _role;
    public GameObject _name;
    public GameObject _newWord;
    // public GameObject _oldWord;
    public GameObject _usedWords;
    public GameObject _textPad;
    public GameObject _check;
    public GameObject _cross;
    public GameObject _timeMeter;
    public GameObject _pageNumber;
    public GameObject _positive;
    public GameObject _negative;

    readonly float _REACTIVE_DELAY = 1f; // In seconds, how long are the buttons blocked to prevent double-click.
    // readonly float _JUDGE_TIME = 10f; // In seconds, how long the judge can decide.
    readonly float _REROLL_TIME = 1f / 3f; // what fraction of time is lost on re-roll
    readonly int PHASE_COUNT = 2; // Two phases per player
    Words _words;
    List<UsedWord> _history;
    float _timer;
    int _step_no;
    int PlayerNo { get { return (_step_no / PHASE_COUNT) % StaticData.players; } }
    int RoundNo { get { return _step_no / (PHASE_COUNT * StaticData.players); } }
    bool FirstPlayer { get { return _step_no / PHASE_COUNT == 0; } }
    bool LastPlayer { get { return RoundNo + 1 >= StaticData.rounds && PlayerNo == StaticData.players - 1; } }
    bool Narrator { get { return _step_no % 2 == 1; } }
    public int PositiveVotes { get; private set; }
    public int NegativeVotes { get; private set; }
    List<bool> _accepted;
    List<GameObject> _judge_buttons;
    List<int> _last_score;

    void Start()
    {
        StaticData.ResetScore();
        _words = new Words();
        _history = new List<UsedWord>();
        _judge_buttons = new List<GameObject>();
        _accepted = Enumerable.Repeat(false, StaticData.players).ToList();
        _last_score = Enumerable.Repeat(0, StaticData.players).ToList();

        SetIconColor();
        SetPlayerName();

        CreateJudgeButtons();
        SetPanel();
        SetJudgeControls();
    }
    void Update()
    {
        // Timing related progress
        double old_timer = _timer;
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            float fill = (StaticData.seconds - _timer) / StaticData.seconds;
            _timeMeter.transform.localScale = new Vector3(fill, 1f, 1f);
        }
        else if (old_timer > 0 && _timer <= 0)
        {
            Next();
        }
        else
        {
            _timeMeter.transform.localScale = Vector3.one;
        }

        // Judging part
        if (PositiveVotes == 0 && NegativeVotes == 0 && !Narrator)
        {
            Next();
        }

        // Game control
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }

    void CreateJudgeButtons() {
        var prefab = Resources.Load("Judger");
        foreach (int i in Enumerable.Range(0, StaticData.players - 1)) {
            GameObject new_button = Instantiate(prefab) as GameObject;
            new_button.name = "Judger" + i;
            new_button.transform.position = new Vector3(0, -80 * (StaticData.players - 1 - i), 0);
            new_button.transform.SetParent(_judgePanel.transform, false);
            _judge_buttons.Add(new_button);
        }
    }

    void SetPanel()
    {
        _narratorPanel.SetActive(Narrator);
        _judgePanel.SetActive(!Narrator);
        _timer = Narrator && !FirstPlayer && !LastPlayer ? StaticData.seconds : 0;
        _role.GetComponent<Text>().text = Narrator ? "narrator" : "judge";
    }

    void SetScoring(GameObject judger, bool active)
    {
        judger.transform.FindChild("Up").gameObject.SetActive(active);
        judger.transform.FindChild("Down").gameObject.SetActive(active);
    }

    void SetJudgeControls()
    {
        int tested_round = _step_no / PHASE_COUNT;
        int active_count = 0;
        if (tested_round >= StaticData.players) // The first player is the first to judge
        {
            foreach (GameObject judger in _judge_buttons)
            {
                bool to_judge = _history[--tested_round].accepted;
                if (to_judge) active_count++;
                judger.transform.FindChild("Word").FindChild("Text").GetComponent<Text>().text = to_judge ? _history[tested_round].text : "skipped";
                SetScoring(judger, to_judge);
            }
        }
        PositiveVotes = Mathf.Min(active_count, StaticData.players / 2);
        NegativeVotes = active_count - PositiveVotes;
        SetVoteNumbers();
    }

    void SetVoteNumbers()
    {
        _positive.GetComponent<Text>().text = PositiveVotes.ToString();
        _negative.GetComponent<Text>().text = NegativeVotes.ToString();
    }

    void SetText()
    {
        if (FirstPlayer) // First word
        {
            _newWord.GetComponent<Text>().text = "The story how\n" + _words.GetWord(true) + "...";
        }
        else if (LastPlayer) { // Last round, last player.
            _newWord.GetComponent<Text>().text = "...The End.";
        }
        else
        {
            _newWord.GetComponent<Text>().text = ", " + _words.GetConnective() + "...\n" + _words.GetWord(true) + "...";
        }
        _usedWords.GetComponent<Text>().text = GetStoryText();
    }

    void Next()
    {
        _step_no++;
        if (RoundNo >= StaticData.rounds)
        {
            Application.LoadLevel("Score");
        }
        else if (Narrator)
        {
            SetPanel();
            _cross.SetActive(!FirstPlayer && !LastPlayer);
            SetText();
        }
        else
        {
            SetPanel();
            SetIconColor();
            SetPlayerName();
            SetJudgeControls();
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
        foreach (UsedWord word in _history) {
            text += word.text;
        }
        return text.Replace("\n", " ");
    }

    void SetIconColor()
    {
        if (_last_score[PlayerNo] > 0)
        {
            _icon.GetComponent<Image>().color = new Color(0f, 0.5f, 0f);
        } else if (_last_score[PlayerNo] == 0)
        {
            _icon.GetComponent<Image>().color = new Color(0.5f, 0.3f, 0f);
        } else
        {
            _icon.GetComponent<Image>().color = new Color(0.5f, 0f, 0f);
        }
        _last_score[PlayerNo] = 0;
    }

    void SetPlayerName()
    {
        _name.GetComponent<Text>().text = "Player " + (PlayerNo + 1);
    }

    public void Check()
    {
        Finished(true);
        Next();
    }

    public void Cross()
    {
        Finished(false);
        Next();
    }

    public void JudgeClick(int i, int _score_change)
    {
        if (_score_change > 0)
        {
            PositiveVotes--;
        } else
        {
            NegativeVotes--;
        }
        int affected_player = (StaticData.players + PlayerNo - i) % StaticData.players;
        _last_score[affected_player] += _score_change;
        StaticData.score[affected_player] += _score_change;
        SetScoring(_judge_buttons[i], false);
        SetVoteNumbers();
    }
}
