using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Words {
    List<string> _word_list = new List<string>();
    List<string> _connectives = new List<string>();
    List<string> _names = new List<string>();

    public Words()
    {
        foreach (var word_list in StaticData.word_lists.Where(x => StaticData.used_lists[x.Key] == true))
        {
            _word_list.InsertRange(0, word_list.Value);
        }
        foreach (var connective_list in StaticData.connectives_lists)
        {
            _connectives.InsertRange(0, connective_list.Value);
        }
        if ((from word in _word_list where char.IsUpper(word[0]) select word).Count() == 0)
        {
            throw new System.ArgumentException("No nouns for the current game.");
        }
        _names.InsertRange(0, StaticData.names_list);
    }

    /// <summary>
    /// Pick a word from the game's selection. If there's no possible, return empty string.
    /// </summary>
    /// <param name="fresh"></param>
    /// <param name="noun_only"></param>
    /// <returns></returns>
    public string GetWord (bool noun_only)
    {
        var selected = (from word in _word_list where (!noun_only || char.IsUpper(word[0])) select word);
        if (selected.Count() == 0) {
            return "";
        } else
        {
            string word = selected.ElementAt(UnityEngine.Random.Range(0, selected.Count()));
            _word_list.Remove(word);
            return word;
        }
    }

    public string GetConnective()
    {
        return _connectives.ElementAt(UnityEngine.Random.Range(0, _connectives.Count()));
    }

    public string GetName()
    {
        string name = _names.ElementAt(UnityEngine.Random.Range(0, _names.Count()));
        _names.Remove(name);
        return name;
    }
}
