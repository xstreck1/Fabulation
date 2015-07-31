using UnityEngine;
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
        if (StaticData.used_lists.Count(x => x.Value == true) > 0)
        {
            Application.LoadLevel("Game");
        }
        else
        {
            Debug.LogError("No word list selected.");
        }
    }
}
