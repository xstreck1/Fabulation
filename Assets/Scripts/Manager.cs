using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    GameObject _menu;
    GameObject _game;

    // Use this for initialization
    void Start () {
        _menu = GameObject.Find("Canvas");
        _game = GameObject.Find("Game");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        int players = (int) _menu.transform.FindChild("PlayersCount").GetComponent<Slider>().value;
        int points = (int)_menu.transform.FindChild("PointsCount").GetComponent<Slider>().value;
        int seconds = (int)_menu.transform.FindChild("SecondsCount").GetComponent<Slider>().value;
        _menu.SetActive(false);
        _game.GetComponent<Game>().StartGame(players, points, seconds);
    }
}
