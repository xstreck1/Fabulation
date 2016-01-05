using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Score : MonoBehaviour {
    Text _author_name;
    Text _story_text;

    public void Start ()
    {
        _author_name = transform.FindChild("Author").FindChild("Author Text").GetComponent<Text>();
        _story_text = transform.FindChild("Story").FindChild("Story Text").GetComponent<Text>();
        SetResults();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }

    public void SetResults()
    {
        List<string> winners = new List<string>();
        int max_score = GameData.score.Max();
        for (int i = 0; i < Settings.players; i++)
        {
            if (GameData.score[i] == max_score)
            {
                winners.Add(GameData.names[i]);
            }
        }
        string winner = winners.ElementAt(UnityEngine.Random.Range(0, winners.Count()));
        _author_name.text = winner;

        _story_text.text = Settings.story;
    }

    public void FinishGame()
    {
        Application.LoadLevel("Menu");
    }
}
