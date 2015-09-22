using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RateButton : MonoBehaviour {
    public int _scoreChange;
    Button _button;
    int _state; // 1 on, 0 undecided, -1 off
    List<float> _scale_factors = new List<float> { 0.5f, 0.75f, 1f };

	// Use this for initialization
	void Start ()
    {
        _button = this.GetComponent<Button>();
    }

    void OnEnable()
    {
        Switch(0);
    }

    public void Switch (int new_state)
    {
        _state = new_state;
        float scale = _scale_factors[new_state + 1];
        transform.localScale = Vector3.one * scale;
    }
}
