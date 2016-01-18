using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {

    public void Back()
    {
        Application.LoadLevel("Menu");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void JAC()
    {
        Application.OpenURL("http://justaconcept.org/");
    }

    public void CC()
    {
        Application.OpenURL("https://creativecommons.org/licenses/by-nc/3.0/");
    }
}
