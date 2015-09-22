using UnityEngine;
using UnityEngine.UI;
using System;

public class Judger : MonoBehaviour {
    GameObject _up_object;
    RateButton _up_rate;
    GameObject _down_object;
    RateButton _down_rate;
    Game _game;
    int _my_no;
    int _configuration; // 1 for up, 0 for non, -1 for down

    // Use this for initialization
    void Awake () {
        _up_object = transform.FindChild("Up").gameObject;

        _down_object = transform.FindChild("Down").gameObject;
    }

    void Start()
    {
        _up_rate = _up_object.GetComponent<RateButton>();
        _up_object.GetComponent<Button>().onClick.AddListener(UpClick);
        _down_rate = _down_object.GetComponent<RateButton>();
        _down_object.GetComponent<Button>().onClick.AddListener(DownClick);

        _game = GameObject.Find("Canvas").GetComponent<Game>();
        _my_no = (int)Char.GetNumericValue(name, name.Length - 1);

    }

    public void OnEnable()
    {
        _up_object.SetActive(true);
        _down_object.SetActive(true);
        _configuration = 0;
    }

    public void Disable()
    {
        _up_object.SetActive(false);
        _down_object.SetActive(false);
    }

	void UpClick () {
        if (_configuration == 1)
        {
            _up_rate.Switch(0);
            _down_rate.Switch(0);
            _game.JudgeClick(1, 0);
            _configuration = 0;
        }
        else if (_game.PositiveVotes > 0)
        {
            _up_rate.Switch(1);
            _down_rate.Switch(-1);
            _game.JudgeClick(-1, _configuration == 0 ? 0 : 1);
            _configuration = 1;
        }
	}

    void DownClick()
    {
        if (_configuration == -1)
        {
            _up_rate.Switch(0);
            _down_rate.Switch(0);
            _game.JudgeClick(0, 1);
            _configuration = 0;
        }
        else if (_game.NegativeVotes > 0)
        {
            _up_rate.Switch(-1);
            _down_rate.Switch(1);
            _game.JudgeClick(_configuration == 0 ? 0 : 1, -1);
            _configuration = -1;
        }
    }

    public int GetScore()
    {
        if (_configuration == 1)
        {
            return _up_rate._scoreChange;
        }
        else if(_configuration == 0)
        {
            return 0;
        }
        else
        {
            return _down_rate._scoreChange;
        }
    }
}
