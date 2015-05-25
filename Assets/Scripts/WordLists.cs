using UnityEngine;
using System.Collections.Generic;
using System;

// Word list obtained from http://www.desiquintans.com/nounlist

public static class WordLists
{
    public static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
    public static string[] lists = new string[] { "basic" };
    private static readonly string _LISTS_FOLDER = "WordLists";

    static WordLists()
    {
        foreach (string list_name in lists)
        {
            TextAsset text_asset = Resources.Load(_LISTS_FOLDER + "/" + list_name ) as TextAsset;
            dictionary[list_name] = text_asset.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
    }

    public static string SelectList(List<string> lists)
    {
        return lists[UnityEngine.Random.Range(0, lists.Count)];
    }

    public static string SelectWord(string list_name)
    {
        string[] list = dictionary[list_name]; 
        return list[UnityEngine.Random.Range(0, list.Length)];
    }
};