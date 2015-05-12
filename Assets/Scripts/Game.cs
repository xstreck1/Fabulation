using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Game : MonoBehaviour
{ 
    public static readonly Color[] colors = {
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

    Manager _manager;
    Image _background;
    Text _cue;
    Text _player_name;

    List<int> _players_points;

    float _time;
    float _timer;
    int _current_player;
    
    void Awake()
    {
        _manager = GameObject.Find("Manager").GetComponent<Manager>();
        _background = transform.FindChild("Background").GetComponent<Image>();
        _cue = transform.FindChild("Cue").GetComponent<Text>();
        _player_name = transform.FindChild("PlayerName").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            Handheld.Vibrate();
            _players_points[_current_player] -= 1;
            if (_players_points[_current_player] == 0)
            {
                _manager.EndGame(_players_points);
            }
            else
            {
                NextPlayer();
            }
        }
    }

    void NewPlayer(int new_player)
    {
        _current_player = new_player;
        _timer = _time;
        _background.color = colors[_current_player];
        _player_name.text = "Player: " + (_current_player+1).ToString();
        _cue.text = WordList.words[Random.Range(0, WordList.words.Count())];
    }

    public void StartGame(int players, int points, int seconds)
    {
        _players_points = Enumerable.Repeat(points, players).ToList();
        _time = seconds;
        NewPlayer(0);
    }
    public void NextPlayer()
    {
        NewPlayer((_current_player + 1) % _players_points.Count());
    }
}
