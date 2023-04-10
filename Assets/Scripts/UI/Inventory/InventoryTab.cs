using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class InventoryTab : MonoBehaviour
{
    public Button[] buttons;
    public Button nowButton;

    private void Start()
    {
        foreach (var button in buttons)
        {
            button.targetGraphic.color = Color.white;
        }
        nowButton = buttons[0];
        nowButton.targetGraphic.color = Color.green;
    }

    public void OnTabChanged(Button nextButton)
    {
        if (nowButton == null) return;

        nowButton.targetGraphic.color = Color.white;
        nowButton = nextButton;
        nowButton.targetGraphic.color = Color.green;
    }
    public void OnEnable()
    {
        foreach (var button in buttons)
        {
            button.targetGraphic.color = Color.white;
        }
        nowButton = buttons[0];
        nowButton.targetGraphic.color = Color.green;
    }
}