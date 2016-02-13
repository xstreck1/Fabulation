using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct UsedWord
{
    public int round;
    public int player;
    public string text;
}

public static class GameData
{
    static public List<int> score = new List<int>();
    static public List<string> names = new List<string>();
    static int _step_no; // Incremented with each scene change
    static public int PhaseCount { get { return Settings.IsCompetitive ? 3 : 2; } } // Three phases per player
    static string winner = "";

    static public Words words;
    static public List<UsedWord> history;
    static public string title;

    static public int PlayerNo { get { return (_step_no / PhaseCount) % Settings.PlayerCount; } }
    static public int RoundNo { get { return _step_no / (PhaseCount * Settings.PlayerCount); } }
    static public bool FirstPlayer { get { return _step_no / PhaseCount == 0; } }
    static public bool LastPlayer { get { return RoundNo + 1 >= Settings.RoundCount && PlayerNo == Settings.PlayerCount - 1; } }
    static public bool GameEnded { get { return _step_no >= PhaseCount * Settings.PlayerCount * Settings.RoundCount; } }

    static public bool HasNames { get; set; }

    static GameData()
    {
        Reset();
        // Create dummy data
        history = Enumerable.Repeat(new UsedWord { text = "Dummy Text." }, Settings.PlayerCount).ToList();
        names = Enumerable.Repeat("Dummy Player", Settings.PlayerCount).ToList();
        title = "The Dummy Story";
        if (Application.loadedLevelName == "Speaker")
        {
            _step_no = PhaseCount - 2;
        }
        else if (Application.loadedLevelName == "NextPlayer")
        {
            _step_no = PhaseCount - 3;
        }
    }

    static public void Reset()
    {
        if (Settings.IsCompetitive)
            _step_no = 0;
        winner = "";
        score = Enumerable.Repeat(0, Settings.PlayerCount).ToList();
        names = Enumerable.Repeat("", Settings.PlayerCount).ToList();
        words = new Words();
        history = new List<UsedWord>();
        HasNames = false;
    }

    static public void Next()
    {
        _step_no++;

        if (PhaseCount == 3)
        {
            switch (_step_no % PhaseCount)
            {
                case 0:
                    if (RoundNo == 0)
                    {
                        if (!GameData.HasNames)
                        {
                            Application.LoadLevel("Names");
                        }
                        else
                        {
                            _step_no++;
                            Application.LoadLevel("Speaker");
                        }
                    }
                    else if (!GameData.HasNames)
                    {
                        GameData.HasNames = true;
                        _step_no = 1;
                        Application.LoadLevel("Speaker");
                    }
                    else if (GameEnded)
                    {
                        Application.LoadLevel("Score");
                    }
                    else
                    {
                        Application.LoadLevel("Judge");
                    }
                    break;
                case 1:
                    if (RoundNo == 0)
                    {
 
                        _step_no ++;
                        Application.LoadLevel("NextPlayer");
                    }
                    else {
                        Application.LoadLevel("Speaker");
                    }
                    break;
                case 2:
                    Application.LoadLevel("NextPlayer");
                    break;
            }
        }
        else {
            switch (_step_no % PhaseCount)
            {
                case 0:
                    if (GameEnded)
                    {
                        Application.LoadLevel("Score");
                    }
                    else
                    {
                        Application.LoadLevel("Speaker");
                    }
                    break;
                case 1:
                    Application.LoadLevel("NextPlayer");
                    break;
            }
        }
    }

    public static string GetStoryText()
    {
        string text = "";
        foreach (UsedWord word in GameData.history)
        {
            text += word.text;
        }
        return text.Replace("\n", " ");
    }

    public static string getWinner()
    {
        if (winner == "")
        {
            List<string> winners = new List<string>();
            int max_score = GameData.score.Max();
            for (int i = 0; i < Settings.PlayerCount; i++)
            {
                if (GameData.score[i] == max_score)
                {
                    winners.Add(GameData.names[i]);
                }
            }
            winner = winners.ElementAt(UnityEngine.Random.Range(0, winners.Count()));
        }
        return winner;
    }
}
