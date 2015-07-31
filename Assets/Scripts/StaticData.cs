using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct GameMode
{
    public string name;
    public bool using_lives;
}



public static class StaticData
{
    private const string _LISTS_FOLDER = "WordLists";

    static public List<GameMode> mode_list = new List<GameMode>();
    static public Dictionary<string, List<string> > word_lists = new Dictionary<string, List<string>>();
    static public Dictionary<string, bool> used_lists = new Dictionary<string, bool>();
    static public int current_mode_ID = 0;
    static public int players = 2;
    static public int points = 10;
    static public int lives = 2;
    static public int seconds = 30;
    static public List<int> score = new List<int>();

    static StaticData()
    {
        mode_list = new List<GameMode>()
        {
            new GameMode { name = "Last Man Standing",  using_lives = true},
            new GameMode { name = "Collaboration",  using_lives = false},
            new GameMode { name = "Sudden death",  using_lives = true},
        };
        string[] used_genres = { "adventure", "basic", "crimi", "drama", "fantasy", "fairytale", "horror", "sci-fi", "western" };
        // Load the dictionaries
        foreach (string genre_name in used_genres) {
            used_lists.Add(genre_name, false);
            TextAsset text_asset = Resources.Load(_LISTS_FOLDER + "/" + genre_name) as TextAsset;
            word_lists[genre_name] = text_asset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None).ToList<string>();
            word_lists[genre_name].RemoveAll(x => x.Length == 0 || x[0] == '#'); // Remove all the lines that start with #
        }
        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(points, players).ToList();
    }
}
