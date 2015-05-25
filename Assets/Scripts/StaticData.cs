using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class StaticData
{
    static public int players { get; set; }
    static public int points { get; set; }
    static public int seconds { get; set; }
    static public List<int> score { get; set; }

    static StaticData() {
        players = 2;
        points = 2;
        seconds = 30;
        ResetScore();
    }

    static public void ResetScore()
    {
        score = Enumerable.Repeat(0, players).ToList();
    }


    //public void StartGame()
    //{
    //    int players = (int) _menu.transform.FindChild("PlayersCount").GetComponent<Slider>().value;
    //    int points = (int)_menu.transform.FindChild("PointsCount").GetComponent<Slider>().value;
    //    int seconds = (int)_menu.transform.FindChild("SecondsCount").GetComponent<Slider>().value;
    //    activateGame();
    //    _game.GetComponent<Game>().StartGame(players, points, seconds);
    //}

    //public void EndGame(List<int> score)
    //{
    //    activateScore();
    //    _score.GetComponent<Score>().setScore(score);
    //}

    //public void GoToMenu()
    //{
    //    _score.SetActive(false);
    //    _menu.SetActive(true);
    //}
}
