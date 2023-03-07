using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(string itemName, string itemCount = null)
    {
        itemNameText.text = itemName;
        if (itemCountText != null)
            itemCountText.text = itemCount;
    }
}