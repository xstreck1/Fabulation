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

    GameObject _check, _cross, _arrow;
    Text _previous;
    Transform[] _actors = new Transform[2]; // First judge, than player
    Image[] _backgrounds = new Image[2];
    Text[] _cues = new Text[2];
    Text[] _player_names = new Text[2];
    Transform[] _meters = new Transform[2];

    string _current_cue;
    float _REACTIVE_DELAY = 1f; // In seconds, how long are the buttons blocked to prevent double-click.
    float _timer;
    int[] _actor_ids = new int[2]; // Current acting players

    void Awake()
    {
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
        StaticData.ResetScore();
        NewPlayer(0, 1);
    }

    void Update()
    {
        float new_time = _timer - Time.deltaTime;

        if (_timer > 0)
        {
            // Display arrow
            if (new_time <= 0)
            {
                Handheld.Vibrate();
                _check.SetActive(false);
                _cross.SetActive(false);
                _arrow.SetActive(true);
            }
            // Change the meter
            _meters[0].localScale = _meters[1].localScale = new Vector3((StaticData.seconds - new_time) / StaticData.seconds, 1, 1);
        }

        _timer = new_time;
    }

    public void Check()
    {
        // Prevent double click error
        if (StaticData.seconds - _timer < _REACTIVE_DELAY)
        {
            return;
        }
        else
        {
            StaticData.score[_actor_ids[1]] += 1;
            _previous.text = _current_cue + "\n" + _previous.text;
            if (StaticData.score[_actor_ids[1]] == StaticData.points)
            {
                Application.LoadLevel("Score");
            }
            else
            {
                NextPlayer();
            }
        }
    }

    public void Cross()
    {
        // Prevent double click error
        if (StaticData.seconds - _timer < _REACTIVE_DELAY)
        {
            return;
        }
        else
        {
            NextPlayer();
        }
    }

    public void Arrow()
    {
        _check.SetActive(true);
        _cross.SetActive(true);
        _arrow.SetActive(false);
        NextPlayer();
    }

    void NewPlayer(int judge, int player)
    {
        _actor_ids[0] = judge;
        _actor_ids[1] = player;
        _timer = StaticData.seconds;
        _current_cue = WordList.words[Random.Range(0, WordList.words.Count())];
        foreach (int i in Enumerable.Range(0, 2))
        {
            _backgrounds[i].color = colors[_actor_ids[i]];
            _player_names[i].text = "Player: " + (_actor_ids[i] + 1).ToString();
            _cues[i].text = _current_cue;
        }
    }

    public void NextPlayer()
    {
        NewPlayer((_actor_ids[0] + 1) % StaticData.players, (_actor_ids[1] + 1) % StaticData.players);
    }
}
