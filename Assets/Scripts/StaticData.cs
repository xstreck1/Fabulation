using System.Collections.Generic;
using System.Collections;
using System.Linq;

public struct GameMode
{
    public int ID;
    public string name;
    public bool using_lives;
}

public static class StaticData
{

    static public List<GameMode> mode_list;
    static public int current_mode_ID;
    static public int players;
    static public int points;
    static public int lives;
    static public int seconds;
    static public List<string> lists;
    static public List<int> score;

    static StaticData()
    {
        mode_list = new List<GameMode>
        {
            new GameMode { ID = 0, name = "Last Man Standing",  using_lives = true},
            new GameMode { ID = 1, name = "Collaboration",  using_lives = false},
            new GameMode { ID = 1, name = "Sudden death",  using_lives = true},
        };
        current_mode_ID = 0;
        players = 2;
        points = 10;
        lives = 2;
        seconds = 30;
        lists = new List<string>() { "basic" };
        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(points, players).ToList();
    }
}
