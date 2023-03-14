using TMPro;
using UnityEngine;

public class StageEnemy : MonoBehaviour
{
    public TextMeshProUGUI countTxt;
    public int count;

    public int Count {
        get { return count; }
        set {
            if (value < 0)
                return;
            count = value;
            countTxt.text = count.ToString();
        }
    }

    [ContextMenu("Test/DieEnemy")]
    public int DieEnemy() => --Count;
    [ContextMenu("Test/AddEnemy")]
    public int AddEnemy() => ++Count;

}
