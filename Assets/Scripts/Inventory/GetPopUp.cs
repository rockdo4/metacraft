using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetPopUp : MonoBehaviour
{
    public Slider countSlider;
    public TextMeshProUGUI count;
    int boxCount;
    int addCount;
    Item nowItem;
    Item useItem;

    public RewardItem viewItem;

    public void OnClickCancle()
    {
        gameObject.SetActive(false);
    }

    public void OnClickGet()
    {
        nowItem.count = int.Parse(count.text) * addCount;

        GameManager.Instance.inventoryData.AddItem(nowItem);
        GameManager.Instance.inventoryData.UseItem(useItem, int.Parse(count.text));
        OnClickCancle();
    }

    public void Set(int boxCount,int addCount, Item nowItem, Item useItem)
    {
        this.boxCount = boxCount;
        this.addCount = addCount;
        this.nowItem = nowItem;
        this.useItem = useItem;

        countSlider.value = 0;
        countSlider.maxValue = boxCount;

        viewItem.SetData(nowItem);
    }

    public void SetCount(Slider slider)
    {
        count.text = ((int)(slider.value)).ToString();
    }

    public void OnClickAddCountButton(Slider slider)
    {
        slider.value = (int)(slider.value) +1;
    }
    public void OnClickMinCountButton(Slider slider)
    {
        slider.value = (int)(slider.value) - 1;
    }
}
