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
    GameObject[] _previous = new GameObject[2];
    Text[] _previous_text = new Text[2];
    Transform[] _actors = new Transform[2]; // First judge, than player
    Image[] _backgrounds = new Image[2];
    Text[] _cues = new Text[2];
    Text[] _player_names = new Text[2];
    Transform[] _meters = new Transform[2];
    Transform _panel;

    string _current_cue;
    readonly int _PHASE_COUNT = 3;
    readonly float _REACTIVE_DELAY = 1f; // In seconds, how long are the buttons blocked to prevent double-click.
    readonly float _JUDGE_TIME = 10f; // In seconds, how long the judge can decide.
    readonly float _PREPARE_TIME = 0.05f; // In seconds, how long the player can prepare
    double[] _timers = new double[2];
    double[] _time = new double[2];
    int[] _actor_ids = new int[2]; // Current acting players
    int _phase = 0;

    void Awake()
    {
        _panel = transform.FindChild("Panel");
        _actors[0] = _panel.FindChild("Judge");
        _actors[1] = _panel.FindChild("Player");
        _check = _actors[0].FindChild("Content").FindChild("Check").gameObject;
        _cross = _actors[0].FindChild("Content").FindChild("Cross").gameObject;
        _arrow = _actors[1].FindChild("Content").FindChild("Arrow").gameObject;
        foreach (int i in Enumerable.Range(0, 2))
        {
            _previous[i] = _actors[i].FindChild("Content").FindChild("Previous").gameObject;
            _previous_text[i] = _previous[i].GetComponent<Text>();
            _previous_text[i].text = "";
            _backgrounds[i] = _actors[i].FindChild("Background").GetComponent<Image>();
            _player_names[i] = _actors[i].FindChild("Name").FindChild("Text").GetComponent<Text>();
            _cues[i] = _actors[i].FindChild("Cue").FindChild("Text").GetComponent<Text>();
            _meters[i] = _actors[i].FindChild("Timer").FindChild("Meter");
        }
    }

    void Start()
    {
        StaticData.ResetScore();
        Debug.Log(StaticData.lists.Count);
        _phase = 0;
        NewPlayer(0, 1);
        NewPhase();
    }

    void NewPhase()
    {
        switch (_phase)
        {
            case 0:
                _check.SetActive(false);
                _cross.SetActive(false);
                _arrow.SetActive(true);
                _previous[0].SetActive(true);
                _previous[1].SetActive(false);
                foreach (int i in Enumerable.Range(0, 2))
                {
                    _backgrounds[i].color = colors[_actor_ids[i]];
                    _player_names[i].text = (i == 0 ? "JUDGE" : "SPEAK") + ": Player " + (_actor_ids[i] + 1).ToString();
                    _cues[i].text = "";
                }
                _timers[0] = _time[0] = double.PositiveInfinity;
                _timers[1] = _time[1] = _PREPARE_TIME;
                _panel.Rotate(0f, 0f, 180f);
                break;
            case 1:
                foreach (int i in Enumerable.Range(0, 2))
                {
                    _cues[i].text = _current_cue;
                }
                _timers[0] = _time[0] = double.PositiveInfinity;
                _timers[1] = _time[1] = StaticData.seconds;
                break;
            case 2:
                _check.SetActive(true);
                _cross.SetActive(true);
                _arrow.SetActive(false);
                _previous[0].SetActive(false);
                _previous[1].SetActive(true);
                _timers[0] = _time[0] = _JUDGE_TIME;
                _timers[1] = _time[1] = double.PositiveInfinity;
                break;
            case 3:
                _cross.SetActive(false);
                _timers[0] = _time[0] = double.PositiveInfinity;
                _timers[1] = _time[1] = double.PositiveInfinity;
                break;
        }
    }

    void nextPhase()
    {
        _phase = _phase + 1 % _PHASE_COUNT;
        NewPhase();
    }

    void Update()
    {
        foreach (int i in Enumerable.Range(0, 2))
        {
            if (_timers[i] == double.PositiveInfinity)
            {
                _meters[i].localScale = Vector3.one;
            }
            else
            {
                // Finish phase
                if (_timers[i] < 0)
                {
                    nextPhase();
                    Handheld.Vibrate();
                }
                else
                {
                    _timers[i] -= Time.deltaTime;
                    // Move meter
                    _meters[i].localScale = new Vector3((float)((_time[i] - _timers[i]) / _time[i]), 1, 1);
                }
            }
        }
    }

    public void Check()
    {
        foreach (int i in Enumerable.Range(0, 2))
        {
            _previous_text[i].text = _current_cue + "\n" + _previous_text[i].text;
        }
            _phase = 0;
            NextPlayer();
            NewPhase();
    }

    public void Cross()
    {
        StaticData.score[_actor_ids[1]] -= 1;

        if (StaticData.score[_actor_ids[1]] == 0)
        {
            Application.LoadLevel("Score");
        } else
        {
            _phase = 0;
            NextPlayer();
            NewPhase();
        }
    }

    public void Arrow()
    {
        if ((_timers[1] < (_time[1] - _REACTIVE_DELAY)) || _phase != 1)
        {
            nextPhase();
        }
    }

    void NewPlayer(int judge, int player)
    {
        _actor_ids[0] = judge;
        _actor_ids[1] = player;
        _current_cue = WordLists.SelectWord(WordLists.SelectList(StaticData.lists)); // Get the random word
    }

    public void NextPlayer()
    {
        NewPlayer((_actor_ids[0] + 1) % StaticData.players, (_actor_ids[1] + 1) % StaticData.players);
    }
}
