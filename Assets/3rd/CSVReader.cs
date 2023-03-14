using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static readonly string SPLIT_SEMICOLON = @";(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static readonly char[] TRIM_CHARS = { '\"' };

    // New2. Split Text Asset
    public static List<Dictionary<string, object>> SplitTextAsset(TextAsset asset, bool splitComma = true, bool tryParse = true)
    {
        return SplitTokens(asset.text, splitComma, tryParse);
    }

    // New1. Use File Path
    public static List<Dictionary<string, object>> ReadByPath(string path, bool splitComma = true)
    {
        string str = File.ReadAllText(path);
        return SplitTokens(str, splitComma);
    }
    public static List<Dictionary<string, object>> ReadByStreamReaderPath(string path, bool splitComma = true, bool tryParse = true)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(stream);
        string str = reader.ReadToEnd();
        return SplitTokens(str, splitComma, tryParse);
    }

    // Legacy. Use Resource folder
    public static List<Dictionary<string, object>> Read(string file, bool splitComma = true)
    {
        TextAsset data = Resources.Load(file) as TextAsset;
        return SplitTokens(data.text, splitComma);
    }

    private static List<Dictionary<string, object>> SplitTokens(string data, bool splitComma = true, bool tryParse = true)
    {
        var list = new List<Dictionary<string, object>>();
        var lines = Regex.Split(data, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], splitComma ? SPLIT_RE : SPLIT_SEMICOLON);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], splitComma ? SPLIT_RE : SPLIT_SEMICOLON);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                if (tryParse)
                {
                    if (int.TryParse(value, out int n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out float f))
                    {
                        finalvalue = f;
                    }
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}