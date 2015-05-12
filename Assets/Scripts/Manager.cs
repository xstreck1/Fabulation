using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Manager : MonoBehaviour {
    public GameObject _menu;
    public GameObject _game;
    public GameObject _score;

    public void activateMenu()
    {
        _game.SetActive(false);
        _score.SetActive(false);
        _menu.SetActive(true);
    }

    public void activateGame()
    {
        _score.SetActive(false);
        _menu.SetActive(false);
        _game.SetActive(true);
    }

    public void activateScore()
    {
        _menu.SetActive(false);
        _game.SetActive(false);
        _score.SetActive(true);
    }

    public void StartGame()
    {
        int players = (int) _menu.transform.FindChild("PlayersCount").GetComponent<Slider>().value;
        int points = (int)_menu.transform.FindChild("PointsCount").GetComponent<Slider>().value;
        int seconds = (int)_menu.transform.FindChild("SecondsCount").GetComponent<Slider>().value;
        activateGame();
        _game.GetComponent<Game>().StartGame(players, points, seconds);
    }

    public void EndGame(List<int> players_points)
    {
        activateScore();
        _score.GetComponent<Score>().setScore(players_points);
    }

    public void GoToMenu()
    {
        _score.SetActive(false);
        _menu.SetActive(true);
    }
}
