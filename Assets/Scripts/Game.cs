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
    GameObject _check, _cross, _arrow;
    Text _previous;
    Transform[] _actors = new Transform[2]; // First judge, than player
    Image[] _backgrounds = new Image[2];
    Text[]  _cues = new Text[2];
    Text[] _player_names = new Text[2];
    Transform[] _meters = new Transform[2];

    List<int> _players_points;

    string _current_cue;
    float _ROUND_TIME;
    float _timer;
    int[] _actor_ids = new int[2];

    void Awake()
    {
        _manager = GameObject.Find("Manager").GetComponent<Manager>();
        _actors[0] = transform.FindChild("Judge");
        _actors[1] = transform.FindChild("Player");
        _check = _actors[0].FindChild("Content").FindChild("Check").gameObject;
        _cross = _actors[0].FindChild("Content").FindChild("Cross").gameObject;
        _arrow = _actors[0].FindChild("Content").FindChild("Arrow").gameObject;
        _arrow.SetActive(false);
        _previous = _actors[1].FindChild("Content").FindChild("Previous").gameObject.GetComponent<Text>();
        _previous.text = "";
        foreach (int i in Enumerable.Range(0, 2))
        {
            _backgrounds[i] = _actors[i].FindChild("Background").GetComponent<Image>();
            _player_names[i] = _actors[i].FindChild("Name").FindChild("Text").GetComponent<Text>();
            _cues[i] = _actors[i].FindChild("Cue").FindChild("Text").GetComponent<Text>();
            _meters[i] = _actors[i].FindChild("Timer").FindChild("Meter");
        }
    }

    void Start()
    {
#if UNITY_EDITOR
        StartGame(2, 2, 5);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            Handheld.Vibrate();
            _check.SetActive(false);
            _cross.SetActive(false);
            _arrow.SetActive(true);
        } else
        {
            _meters[0].localScale = _meters[1].localScale = new Vector3((_ROUND_TIME - _timer) / _ROUND_TIME, 1, 1);
        }
    }

    public void Check()
    {
        _previous.text = _current_cue + "\n" + _previous.text;
        NextPlayer();
    }

    public void Cross()
    {
        _players_points[_actor_ids[1]] -= 1;
        if (_players_points[_actor_ids[1]] == 0)
        {
            _manager.EndGame(_players_points);
        }
        else
        {
            NextPlayer();
        }
    }

    public void Arrow()
    {
        _players_points[_actor_ids[1]] -= 1;
        if (_players_points[_actor_ids[1]] == 0)
        {
            _manager.EndGame(_players_points);
        }
        else
        {
            _check.SetActive(true);
            _cross.SetActive(true);
            _arrow.SetActive(false);
            NextPlayer();
        }
    }

    void NewPlayer(int judge, int player)
    {
        _actor_ids[0] = judge;
        _actor_ids[1] = player;
        _timer = _ROUND_TIME;
        _current_cue = WordList.words[Random.Range(0, WordList.words.Count())];
        foreach (int i in Enumerable.Range(0, 2))
        {
            _backgrounds[i].color = colors[_actor_ids[i]];
            _player_names[i].text = "Player: " + (_actor_ids[i] + 1).ToString();
            _cues[i].text = _current_cue;
        }
    }

    public void StartGame(int players, int points, int seconds)
    {
        _players_points = Enumerable.Repeat(points, players).ToList();
        _ROUND_TIME = seconds;
        NewPlayer(0,1);
    }
    public void NextPlayer()
    {
        NewPlayer((_actor_ids[0] + 1) % _players_points.Count(), (_actor_ids[1] + 1) % _players_points.Count());
    }
}
