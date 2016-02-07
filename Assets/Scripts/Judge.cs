using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;


public class Judge : MonoBehaviour
{
    public GameObject _instructionsText;
    public GameObject _votes;
    public GameObject _votesText;
    public GameObject _speak;
    public GameObject _confirmPanel;

    List<GameObject> _judgers;

    int Remaining
    {
        get
        {
            return Settings.VotesAvailable - _judgers.Sum(j => j.GetComponent<Toggle>().isOn ? 1 : 0);
        }
    }

    void Start()
    {
        _judgers = new List<GameObject>();

        SetPlayerName();
        CreateJudgeButtons();
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
    }

    void CreateJudgeButtons()
    {
        var prefab = Resources.Load("Judger");
        foreach (int i in Enumerable.Range(0, Settings.players - 1))
        {
            if (i > 9)
            {
                // To fix this, change the way the judger obtain its ID (currently from its Name)
                throw new System.IndexOutOfRangeException("Max 10 players allowed. More would break the code.");
            }
            GameObject new_judger = Instantiate(prefab) as GameObject;
            new_judger.name = "Judger" + (i);
            // new_judger.GetComponent<RectTransform>().position = new Vector3(0, -100 - 80 * i, 0);
            new_judger.transform.SetParent(transform.FindChild("Judgers"), false);
            new_judger.GetComponent<Toggle>().onValueChanged.AddListener((value) => JudgeClick(value));
            Text juger_text = new_judger.transform.FindChild("Word").FindChild("Text").GetComponent<Text>();
            // order the last (Settings.players - 1) words such that the one used the lates is on the bottom
            juger_text.text = GameData.history[GameData.history.Count - Settings.players + i + 1].text;
            _judgers.Add(new_judger);
        }

        SetVoteNumbers();
    }

    void SetVoteNumbers()
    {
        // TODO set bottom
        if (Remaining > 0)
        {
            _speak.SetActive(false);
            _votes.SetActive(true);
            string vote_word = Remaining == 1 ? " VOTE " : " VOTES ";
            _votesText.GetComponent<Text>().text = Remaining.ToString() + vote_word + "REMAINING";
        }
        else
        {
            _speak.SetActive(true);
            _votes.SetActive(false);
        }
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

    public void Speak()
    {
        for (int i = 0; i < _judgers.Count; i++)
        {
            // The player that spoke the earliest is the successor of the curent player
            int affected_player = (GameData.PlayerNo + i + 1) % Settings.players;

            Toggle toggle = _judgers[i].GetComponent<Toggle>();
            if (toggle.isOn)
            {
                GameData.score[affected_player]++;
            }
        }
        GameData.Next();
    }

    public void JudgeClick(bool on)
    {
        
        if (on && Remaining == 0)
        {
            _judgers.ForEach(j => j.GetComponent<Toggle>().enabled = j.GetComponent<Toggle>().isOn); // Disable those not on
        }
        else if (!on && Remaining == 1)
        {
            _judgers.ForEach(j => j.GetComponent<Toggle>().enabled = true);
        }
        SetVoteNumbers();
    }
}
