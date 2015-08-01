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

    public GameObject _panel;
    public GameObject _icon;
    public GameObject _role;
    public GameObject _name;
    public GameObject _newWord;
    // public GameObject _oldWord;
    public GameObject _usedWords;
    public GameObject _textPad;
    public GameObject _check;
    public GameObject _cross;
    public GameObject _timeMeter;

    readonly float _REACTIVE_DELAY = 1f; // In seconds, how long are the buttons blocked to prevent double-click.
    // readonly float _JUDGE_TIME = 10f; // In seconds, how long the judge can decide.
    readonly float _REROLL_TIME = 1f/3f; // what fraction of time is lost on re-roll

    Words _words;
    string _current_old = "";
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
        _words = new Words();

        SetIconColor();
        SetPlayerName();
        _role.GetComponent<Text>().text = "narrator";
        _timer = StaticData.seconds;

        // Simulate accepting a word
        _current_old = _words.GetWord(true, true);
        _words.Finished(_current_old, true);
        PopulateUsedWords();

        _newWord.GetComponent<Text>().text = _words.GetWord(true, false);
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
        else if (old_timer > 0 && _timer <= 0)
        {
            Next();
        }
        else
        {
            _timeMeter.transform.localScale = Vector3.one;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }

    void Next()
    {
        if (IsGameFinished())
        {
            Application.LoadLevel("Score");
        }
        _timer = 0; // Reset the timer
        _narrator = !_narrator;
        Text role_text = _role.GetComponent<Text>();
        if (_narrator)
        {
            role_text.text = "narrator";
            _timer = StaticData.seconds;
            _current_old = _words.GetWord(false, true);
            _newWord.GetComponent<Text>().text = _words.GetWord(true, false);
            PopulateUsedWords();
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
        do
        {
            _player_i = (_player_i + 1) % StaticData.players;
        } while (StaticData.mode_list[StaticData.current_mode_ID].using_lives && StaticData.score[_player_i] == 0);
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

    bool IsGameFinished()
    {
        GameMode mode = StaticData.mode_list[StaticData.current_mode_ID];
        if (mode.using_lives)
        {
            if (mode.skipping_players)
            {
                return StaticData.score.Where(x => x != 0).Count() <= 1; // At most one player left
            }
            else
            {
                return StaticData.score.Where(x => x != 0).Count() < StaticData.players; // At least one player dead
            }
        } 
        else
        {
            return StaticData.score.Sum() >= StaticData.points; // Points counted
        }
    }


    public void Check()
    {
        if (_narrator)
        {
            Next();
        }
        else
        {
            _words.Finished(_newWord.GetComponent<Text>().text, true);
            if (!StaticData.mode_list[StaticData.current_mode_ID].using_lives)
            {
                StaticData.score[_last_player_i] += 1;
            }
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
                _newWord.GetComponent<Text>().text = _words.GetWord(true, false);
            }
            else
            {
                Next();
            }
        }
        else
        {
            _words.Finished(_newWord.GetComponent<Text>().text, false);
            if (StaticData.mode_list[StaticData.current_mode_ID].using_lives)
            {
                StaticData.score[_last_player_i] -= 1;
            }
            Next();
        }
    }

    void PopulateUsedWords()
    {
        // Debug.Log("X: " + _textPad.GetComponent<ScrollRect>().normalizedPosition.x + " Y: " + _textPad.GetComponent<ScrollRect>().normalizedPosition.y);
        string[] words_array = _words.GetUsed().ToArray();
        int _old_word_i = System.Array.IndexOf(words_array, _current_old);
        float scroll_pos = 1 - (((float)_old_word_i) / words_array.Count());
        _textPad.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, scroll_pos);
        words_array[_old_word_i] = "<size=50><color=white>+ " + _current_old + "</color></size>";
        _usedWords.GetComponent<Text>().text = string.Join("\n", words_array);
    }
}
