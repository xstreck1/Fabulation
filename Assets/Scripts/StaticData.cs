using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class StaticData
{
    public enum  GameMode { LastManStanding, SuddenDeath};
    static public GameMode game_mode;
    static public int players;
    static public int points;
    static public int seconds;
    static public List<string> lists;
    static public List<int> score;

    static StaticData()
    {
        game_mode = GameMode.LastManStanding;
        players = 2;
        points = 2;
        seconds = 30;
        lists = new List<string>() { "basic" };
        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(points, players).ToList();
    }
}
