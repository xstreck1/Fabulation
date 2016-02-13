using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextPlayer : MonoBehaviour {
    Text _pass_text;

#if UNITY_EDITOR
    readonly float PASS_TIME = 0.1f; // Shorter for debugging purposes
#else
    readonly float PASS_TIME = 1.5f;
#endif

    float _timer;
    
    void Start ()
    {   
        _pass_text = GameObject.Find("Text").GetComponent<Text>();
        if (GameData.LastPlayer)
        {
            if (Settings.IsCompetitive)
            {
                _timer = PASS_TIME * 3;
                _pass_text.text = "the credit for the story goes to";
            }
            else
            {
                _timer = PASS_TIME;
                _pass_text.text = "congragulations!\nyou finished a game";
            }
        }
        else
        {
            _pass_text.text = "next player\npass on the device";
            _timer = PASS_TIME;
        }
    }
	
	void Update ()
    {
        float _old_timer = _timer;
        _timer -= Time.deltaTime;

        if (_timer <= PASS_TIME*2 && _old_timer > PASS_TIME * 2)
        {
            _pass_text.text += "\n" + GameData.getWinner() + ",";
        }
        if (_timer <= PASS_TIME && _old_timer > PASS_TIME)
        {
            _pass_text.text += "\nwho made up the best parts of this story";
        }
        if (_timer < 0)
        {
            GameData.Next();
        }
    }
}
