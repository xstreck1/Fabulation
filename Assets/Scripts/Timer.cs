using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    Speaker _speaker;
    Image _image;

    readonly float VIBRATE_FIRST = 0.7f; //  
    readonly float VIBRATE_SECOND = 0.85f; //  
    int vib_count;

    // Use this for initialization
    void Start () {
        vib_count = 0;
        _speaker = GameObject.Find("Canvas").GetComponent<Speaker>();
        _image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_speaker.Timer > 0)
        {
            float progress = (Settings.Seconds - _speaker.Timer) / Settings.Seconds;

            transform.localScale = new Vector3(progress, 1f, 1f);
            _image.color = new Color(progress / 2, 0f, 0f, 0.75f);

            if ((vib_count == 0 && progress >= VIBRATE_FIRST) || (vib_count ==  1 && progress >= VIBRATE_SECOND))
            {
                vib_count++;
                Handheld.Vibrate();
            }
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
