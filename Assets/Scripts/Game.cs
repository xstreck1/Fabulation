using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour {
    List<int> _players_points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame (int players, int points, int seconds)
    {
        _players_points = Enumerable.Repeat(points, players).ToList();
    }
}
