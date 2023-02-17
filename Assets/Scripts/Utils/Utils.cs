using System.Collections.Generic;
using System.Linq;

public class Utils
{
    public static Dictionary<T, K> ListToDictionary<T, K>(List<T> keys, List<K> values)
    {
        Dictionary<T, K> dict = keys.Zip(values, (t, k) => new { t, k }).ToDictionary(x => x.t, x => x.k);
        return dict;
    }
}