﻿using UnityEngine;
using System.Linq;

public class Genres : MonoBehaviour {
	void Start ()
    {
	}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }

    void StartGame()
    {
        if (Settings.used_lists.Count(x => x.Value == true) > 0)
        {
            GameData.Reset();
            if (Settings.Competetive)
            {
                Application.LoadLevel("Names");
            }
            else
            {
                Application.LoadLevel("Speaker");
            }
        }
        else
        {
            Debug.LogError("No word list selected.");
        }
    }
}
