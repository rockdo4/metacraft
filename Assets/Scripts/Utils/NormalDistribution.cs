using System.Collections.Generic;
using UnityEngine;

public class NormalDistribution
{
    public static float GetData((float min, float max) tuple)
    {
        return GetData(tuple.min, tuple.max);
    }

    public static float GetData(float min, float max)
    {
        float sum = 0;
        int i;
        int count = 2;
        (float x, float y)[] values = new (float x, float y)[count];

        for (i = 0; i < values.Length; i++)
            values[i] = (min / count, max / count);

        for (i = 0; i < values.Length; i++)
            sum += Random.Range(values[i].x, values[i].y);

        return sum;
    }

    public static Dictionary<int, int> GetRange(int tryCount, int min, int max)
    {
        Dictionary<int, int> result = new();

        for (int i = 0; i < tryCount; i++)
        {
            int num = (int)(GetData(min, max) + 0.5f);
            if (result.ContainsKey(num))
                result[num]++;
            else
                result.Add(num, 1);
        }
        return result;
    }
}