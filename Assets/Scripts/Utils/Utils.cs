using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Utils
{
    public static Dictionary<T, K> ListToDictionary<T, K>(List<T> keys, List<K> values)
    {
        Dictionary<T, K> dict = keys.Zip(values, (t, k) => new { t, k }).ToDictionary(x => x.t, x => x.k);
        return dict;
    }

    //Non-duplicate random number generator
    public static List<int> DistinctRandomNumbers(int totalNumber, int requiredNumber)
    {
        List<int> values = new();
        List<int> spwanIndexes = new();
        for (int i = 0; i < totalNumber; i++)
        {
            values.Add(i);
        }

        for (int i = 0; i < requiredNumber; ++i)
        {
            int a = Random.Range(0, values.Count);
            spwanIndexes.Add(values[a]);
            values.RemoveAt(a);
        }
        return spwanIndexes;
    }

    public static void CopyTransform(GameObject gameObject, Transform dest)
    {
        gameObject.transform.SetPositionAndRotation(dest.position, dest.rotation);
        gameObject.transform.localScale = dest.localScale;
    }
}