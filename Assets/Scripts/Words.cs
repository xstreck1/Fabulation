using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Words {
    List<string> _unused = new List<string>();
    List<string> _used = new List<string>();

    Words()
    {
        foreach (var word_list in StaticData.word_lists.Where(x => StaticData.used_lists[x.Key] = true))
        {
            _unused.InsertRange(0, word_list.Value);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
