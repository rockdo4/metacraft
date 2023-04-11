using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class InventoryTab : MonoBehaviour
{
    public Button[] buttons;
    public Button nowButton;

    private Color select = new(0, 171, 179, 255);
    private Color nonSelect = new(178, 178, 178, 255);

    private void Start()
    {
        foreach (var button in buttons)
        {
            button.targetGraphic.color = nonSelect;
        }
        nowButton = buttons[0];
        nowButton.targetGraphic.color = select;
    }

    public void OnTabChanged(Button nextButton)
    {
        if (nowButton == null) return;

        nowButton.targetGraphic.color = nonSelect;
        nowButton = nextButton;
        nowButton.targetGraphic.color = select;
    }
    public void OnEnable()
    {
        foreach (var button in buttons)
        {
            button.targetGraphic.color = nonSelect;
        }
        nowButton = buttons[0];
        nowButton.targetGraphic.color = select;
    }
}