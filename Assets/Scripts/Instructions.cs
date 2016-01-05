using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour
{

    public void GotIt()
    {
        Application.LoadLevel("Genres");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }
    }
}
