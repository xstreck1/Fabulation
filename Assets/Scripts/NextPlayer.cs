using UnityEngine;
using System.Collections;

public class NextPlayer : MonoBehaviour {

#if UNITY_EDITOR
    readonly float PASS_TIME = 0.1f; // Shorter for debugging purposes
#else
    readonly float PASS_TIME = 1.5f;
#endif

    float _timer;
    
    void Start ()
    {
        _timer = PASS_TIME;
    }
	
	void Update ()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            GameData.Next();
        }
    }
}
