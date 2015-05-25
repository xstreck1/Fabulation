using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text _score_text;

	// Use this for initialization
	void Awake () {
        _score_text = transform.FindChild("ScoreText").GetComponent<Text>();
    }
	
	void Start () {
        SetScore();
    }

    public void SetScore()
    {
        _score_text.text = "";
        for (int player_no = 0; player_no < StaticData.players; player_no++)
        {
            _score_text.text += "Player " + (player_no+1).ToString() + ": " + StaticData.score[player_no].ToString() + "\n";
        }
    }

    public void FinishGame()
    {
        Application.LoadLevel("Menu");
    }
}
