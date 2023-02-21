using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    // New2. Split Text Asset
    public static List<Dictionary<string, object>> SplitTextAsset(TextAsset asset)
    {
        return SplitTokens(asset.text);
    }

    // New1. Use File Path
    public static List<Dictionary<string, object>> ReadByPath(string path)
    {
        string str = File.ReadAllText(path);
        return SplitTokens(str);
    }

    // Legacy. Use Resource folder
    public static List<Dictionary<string, object>> Read(string file)
    {
        TextAsset data = Resources.Load(file) as TextAsset;
        return SplitTokens(data.text);
    }

    private static List<Dictionary<string, object>> SplitTokens(string data)
    {
        var list = new List<Dictionary<string, object>>();
        var lines = Regex.Split(data, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                if (int.TryParse(value, out int n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out float f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}