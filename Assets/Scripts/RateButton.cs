using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RateButton : MonoBehaviour {
    public int _scoreChange;
    Button _button;
    Game _game;
    int _my_no;

	// Use this for initialization
	void Start () {
        _my_no = (int) Char.GetNumericValue (transform.parent.name, transform.parent.name.Length - 1);
        _button = this.GetComponent<Button>();
        _button.onClick.AddListener(Click);
        _game = GameObject.Find("Canvas").GetComponent<Game>();
    }
	
    void Update()
    {
        if (_scoreChange > 0)
        {
            if (_game.PositiveVotes <= 0)
            {
                gameObject.SetActive(false);
            }
        } else
        {
            if (_game.NegativeVotes <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Click()
    {
        _game.JudgeClick(_my_no, _scoreChange);
    }
}
