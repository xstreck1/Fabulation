using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderLabel : MonoBehaviour {
    Text _text;
    string _init_text;
    Slider _parents_slider;
    public int ParentValue { get; set; }

    // Use this for initialization
    void Start () {
        _text = this.GetComponent<Text>();
        _init_text = _text.text;
        _parents_slider = transform.parent.GetComponent<Slider>();
        TakeParentValue();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeParentValue()
    {
        ParentValue = (int) _parents_slider.value;
        _text.text = _init_text + ": " + ParentValue.ToString();
    }
}
