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

    Image _BGImage;

    List<int> _players_points;

    float _time = 150f;
    float _timer = 0f;
    int _current_player = 0;

    // Use this for initialization
    void Start()
    {
        _BGImage = transform.FindChild("Background").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = _time;
            _current_player = (_current_player + 1) % colors.Count();
            _BGImage.color = colors[_current_player];
        }

    }

    public void StartGame(int players, int points, int seconds)
    {
        _players_points = Enumerable.Repeat(points, players).ToList();
        _time = seconds;
    }
}
