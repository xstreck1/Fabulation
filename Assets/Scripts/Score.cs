using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Score : MonoBehaviour {
    ScrollRect _scroll_rect;
    Text _story_title;
    Text _author_name;
    Text _story_text;

    public void Start ()
    {
        _story_title = transform.FindChild("Title").FindChild("Title Text").GetComponent<Text>();
        _author_name = transform.FindChild("Title").FindChild("Title Text").GetComponent<Text>();
        _story_text = transform.FindChild("Story").FindChild("Story Text").GetComponent<Text>();
        _scroll_rect = transform.FindChild("Story").GetComponent<ScrollRect>();
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
        string winner = GameData.getWinner();
        _story_title.text = GameData.title;
        _story_text.text = "<size=50>By " + winner + "</size>\n\n" + GameData.GetStoryText();
        
        _scroll_rect.verticalNormalizedPosition = 1f;
    }

    public void FinishGame()
    {
        Application.LoadLevel("Menu");
    }
}
