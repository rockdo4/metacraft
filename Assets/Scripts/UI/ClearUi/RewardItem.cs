using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    int id;
    public int Id {
        get {
            return id;
        }
    }
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(int id, string itemName, string itemCount = null)
    {
        this.id = id;
        itemNameText.text = itemName;
        if (itemCountText != null)
            itemCountText.text = itemCount;
    }
    public void AddCount(string count)
    {
        int nowCount = int.Parse(itemCountText.text);
        itemCountText.text = (nowCount + int.Parse(count)).ToString();
    }
}