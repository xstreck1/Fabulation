using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public void StartGame()
    {
        if (StaticData.lists.Count > 0)
        {
            Application.LoadLevel("Game");
        }
        else
        {
            Debug.LogError("No word list selected.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
