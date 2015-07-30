using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    Text _score_text;

    public void Start ()
    {
        _score_text = transform.FindChild("ScoreText").GetComponent<Text>();
        SetScore();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
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
