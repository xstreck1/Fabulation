using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

// Word list obtained from http://www.desiquintans.com/nounlist

public static class WordLists
{
    public static Dictionary<string, List<String> > dictionary = new Dictionary<string, List<String> >();
    public static string[] lists = new string[] { "adventure", "basic", "crimi", "drama", "fantasy", "fairytale", "horror", "sci-fi", "western" };
    private static readonly string _LISTS_FOLDER = "WordLists";

    static WordLists()
    {
        foreach (string list_name in lists)
        {
            TextAsset text_asset = Resources.Load(_LISTS_FOLDER + "/" + list_name ) as TextAsset;
            dictionary[list_name] = text_asset.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList<String>();
            dictionary[list_name].RemoveAll(x => x[0] == '#'); // Remove all the lines that start with #
        }
    }

    public static string SelectList(List<string> lists)
    {
        return lists[UnityEngine.Random.Range(0, lists.Count)];
    }

    public static string SelectWord(string list_name)
    {
        var list = dictionary[list_name]; 
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
};