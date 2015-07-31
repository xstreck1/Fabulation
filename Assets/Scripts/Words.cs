using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Words {
    List<string> _unused = new List<string>();
    List<string> _used = new List<string>();

    public Words()
    {
        foreach (var word_list in StaticData.word_lists.Where(x => StaticData.used_lists[x.Key] == true))
        {
            _unused.InsertRange(0, word_list.Value);
        }
        if ((from word in _unused where char.IsUpper(word[0]) select word).Count() == 0)
        {
            throw new System.ArgumentException("No nouns for the current game.");
        }
    }

    /// <summary>
    /// Pick a word from the game's selection. If there's no possible, return empty string.
    /// </summary>
    /// <param name="fresh"></param>
    /// <param name="noun_only"></param>
    /// <returns></returns>
    public string GetWord (bool fresh, bool noun_only)
    {
        List<string> list = fresh ? _unused : _used;
        var selected = (from word in list where (!noun_only || char.IsUpper(word[0])) select word);
        return selected.Count() == 0 ? "" : selected.ElementAt(UnityEngine.Random.Range(0, selected.Count()));
    }

    public void Finished(string word, bool accepted)
    {
        _unused.Remove(word);
        if (accepted)
        {
            _used.Insert(0, word);
        }
    }

    public List<string> GetUsed ()
    {
        return _used;
    }
}
