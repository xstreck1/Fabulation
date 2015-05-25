using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class StaticData
{
    static public int players;
    static public int points;
    static public int seconds;
    static public List<string> lists;
    static public List<int> score;

    static StaticData()
    {
        players = 2;
        points = 2;
        seconds = 30;
        lists = new List<string>() { "basic" };
        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(0, players).ToList();
    }
}
