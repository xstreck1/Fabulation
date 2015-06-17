using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

// TODO: If runs out of words, loops forever

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

    public GameObject _panel;
    public GameObject _icon;
    public GameObject _role;
    public GameObject _name;
    public GameObject _newWord;
    public GameObject _oldWord;
    public GameObject _usedWords;
    public GameObject _check;
    public GameObject _cross;
    public GameObject _timeMeter;

    readonly float _REACTIVE_DELAY = 1f; // In seconds, how long are the buttons blocked to prevent double-click.
    // readonly float _JUDGE_TIME = 10f; // In seconds, how long the judge can decide.
    readonly float _REROLL_TIME = 1f/3f; // what fraction of time is lost on re-roll

    List<string> _used_words = new List<string>{};
    bool _narrator = true;
    float _timer = 0;
    int _last_player_i = 0;
    int _player_i = 0;

    void Awake()
    {
    }

    void Start()
    {
        StaticData.ResetScore();

        SetIconColor();
        SetPlayerName();
        PopulateUsedWords();
        _role.GetComponent<Text>().text = "narrator";
        _timer = StaticData.seconds;
        _oldWord.GetComponent<Text>().text = "Let me tell a story about";
        _newWord.GetComponent<Text>().text = getNewWord();
    }

    void Update()
    {
        double old_timer = _timer;
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            float fill = (StaticData.seconds - _timer) / StaticData.seconds;
            _timeMeter.transform.localScale = new Vector3(fill, 1f, 1f);
        }
        else if (old_timer >= 0 && _timer < 0)
        {
            Next();
        }
        else
        {
            _timeMeter.transform.localScale = Vector3.one;
        }
    }

    void Next()
    {
        _narrator = !_narrator;
        Text role_text = _role.GetComponent<Text>();
        if (_narrator)
        {
            role_text.text = "narrator";
            _timer = StaticData.seconds;
            _oldWord.GetComponent<Text>().text = getOldWord();
            _newWord.GetComponent<Text>().text = getNewWord();
        }
        else
        {
            _panel.transform.Rotate(0f, 0f, 180f);
            role_text.text = "audience";
            IncrementPlayer();
        }
    }

    void IncrementPlayer()
    {
        _last_player_i = _player_i;
        _player_i = (_player_i + 1) % StaticData.players;
        SetIconColor();
        SetPlayerName();
    }

    void SetIconColor()
    {
        _icon.GetComponent<Image>().color = colors[_player_i];
    }

    void SetPlayerName()
    {
        _name.GetComponent<Text>().text = "Player " + (_player_i + 1);
    }

    string getNewWord()
    {
        string new_word = "";
        do {
            new_word = WordLists.SelectWord(WordLists.SelectList(StaticData.lists));
        } while (_used_words.Contains(new_word));
        return new_word;
    }

    string getOldWord()
    {
        return _used_words[UnityEngine.Random.Range(0, _used_words.Count)];
    }

    public void Check()
    {
        if (_narrator)
        {
            Next();
        }
        else
        {
            _used_words.Add(_newWord.GetComponent<Text>().text);
            PopulateUsedWords();
            Next();
        }
    }

    public void Cross()
    {
        if (_narrator)
        {
            _timer -= StaticData.seconds * _REROLL_TIME;
            if (_timer > 0)
            {
                _newWord.GetComponent<Text>().text = getNewWord();
            }
            else
            {
                Next();
            }
        }
        else
        {
            StaticData.score[_last_player_i] -= 1;
            if (StaticData.score[_last_player_i] == 0)
            {
                Application.LoadLevel("Score");
            }
            else
            {
                Next();
            }
        }
    }

    void PopulateUsedWords()
    {
        string _used_list = "";
        foreach (string word in _used_words)
        {
            _used_list += word + "\n";
        }
        _usedWords.GetComponent<Text>().text = _used_list;
    }
}
