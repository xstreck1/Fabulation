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
            Application.LoadLevel("Instructions");
        }
    }

    void StartGame()
    {
        if (Settings.used_lists.Count(x => x.Value == true) > 0)
        {
            GameData.Reset();
            Application.LoadLevel("Names");
        }
        else
        {
            Debug.LogError("No word list selected.");
        }
    }
}
