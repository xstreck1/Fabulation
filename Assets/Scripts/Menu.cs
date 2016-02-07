using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        Application.LoadLevel("Genres");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void About()
    {
        Application.LoadLevel("About");
    }
}
