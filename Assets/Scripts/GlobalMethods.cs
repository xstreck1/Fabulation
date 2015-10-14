using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class GlobalMethods
{
    public static string ReplaceName(string input, string new_name)
    {
        string pattern = ">.*<";
        string replacement = ">" + new_name + "<";
        Regex rgx = new Regex(pattern);
        return  rgx.Replace(input, replacement);
    }
}

