using UnityEngine;
using System.Linq;

public class Genres : MonoBehaviour
{
    bool HasGenre { get { return Settings.used_lists.Count(x => x.Value == true) > 0; } }

    public GameObject _button;
    public GameObject _requirement;

    void Start()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }

        _button.SetActive(HasGenre);
        _requirement.SetActive(!HasGenre);
    }

    void StartGame()
    {
        if (HasGenre)
        {
            GameData.Reset();
            if (Settings.IsCompetitive)
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
