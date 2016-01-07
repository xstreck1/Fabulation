using UnityEngine;
using System.Collections;

public class ConfirmPanel : MonoBehaviour {
    public void No()
    {
        gameObject.SetActive(false);
    }

    public void Yes()
    {
        Application.LoadLevel("Genres");
    }
}
