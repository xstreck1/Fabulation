using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticData
{
    private const string _LISTS_FOLDER = "WordLists";

    static public readonly Color[] colors = {
        new Color(1,0,0),
        new Color(0,1,0),
        new Color(0,0,1),
        new Color(1,0.5f,0),
        new Color(0.5f,1,0),
        new Color(1,0,0.5f),
        new Color(0.5f,0,1),
        new Color(0,1,0.5f),
        new Color(0,0.5f,1),
        new Color(0.5f,0.5f,0.5f)
    };

    static public Dictionary<string, List<string>> word_lists = new Dictionary<string, List<string>>();
    static public Dictionary<string, List<string>> connectives_lists = new Dictionary<string, List<string>>();
    static public List<string> names_list = new List<string>();
    static public Dictionary<string, bool> used_lists = new Dictionary<string, bool>();
    static public List<int> score = new List<int>();
    static public List<string> names = new List<string>();

    static public string story;
    static public bool simple = true;
    static public int players = 3;
    static public int rounds = 3;
    static int seconds = 4;
    public static int Seconds {
        get
        {
            switch (seconds) {
                case 1:
                    return 10;
                case 2:
                    return 20;
                case 3:
                    return 30;
                case 4:
                    return 45;
                case 5:
                    return 60;
                case 6:
                    return 90;
                case 7:
                    return 120;
                default:
                    return 0;
            }
        }
        set
        {
            { seconds = value; }
        }
    }

    static StaticData()
    {
        // Keep awake
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Load the dictionaries
        string[] used_genres = { "adventure", "crimi", "drama", "fantasy", "fairytale", "horror", "road story", "sci-fi", "western" };
        foreach (string genre_name in used_genres)
        {
            used_lists.Add(genre_name, true);
            TextAsset text_asset = Resources.Load(_LISTS_FOLDER + "/" + genre_name) as TextAsset;
            word_lists[genre_name] = text_asset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList<string>();
            word_lists[genre_name].RemoveAll(x => x.Length == 0 || x[0] == '#'); // Remove all the lines that start with #
        }
        string[] connectives = { "adverbs", "conjunctions", "prepositions" };
        foreach (string connective_type in connectives)
        {
            TextAsset text_asset = Resources.Load(_LISTS_FOLDER + "/" + connective_type) as TextAsset;
            connectives_lists[connective_type] = text_asset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList<string>();
            connectives_lists[connective_type].RemoveAll(x => x.Length == 0 || x[0] == '#'); // Remove all the lines that start with #
        }
        
        names_list = (Resources.Load("names") as TextAsset).text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList<string>();
        names_list.RemoveAll(x => x.Length == 0 || x[0] == '#'); // Remove all the lines that start with #

        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(0, players).ToList();
        names = Enumerable.Repeat("", players).ToList();
    }
}
