using TMPro;
using UnityEngine;

public class StageEnemy : MonoBehaviour
{
    public TextMeshProUGUI countTxt;
    int count;

    public int Count {
        get { return count; }
        set {
            if (value < 0)
                return;
            count = value;
            countTxt.text = count.ToString();
        }
    }

    [ContextMenu("Test/DimEnemy")]
    public int DimEnemy() => --Count;
    [ContextMenu("Test/AddEnemy")]
    public int AddEnemy() => ++Count;

}
