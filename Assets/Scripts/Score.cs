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
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setScore(List<int> players_points)
    {
        _score_text.text = "";
        for (int player_no = 0; player_no < players_points.Count; player_no++)
        {
            _score_text.text += "Player " + (player_no+1).ToString() + ": " + players_points[player_no].ToString() + "\n";
        }
    }
}
