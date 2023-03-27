using TMPro;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject outline;
    public GameObject chatWindow;
    private TextMeshProUGUI textObject;

    public void SetText(string txt)
    {
        textObject.text = txt;
    }
    public void SetOutline()
    {
        // ?
    }
    public void OnWindow()
    {
        chatWindow.SetActive(true);
    }
    public void OnOutline()
    {
        outline.SetActive(true);
    }
    public void OffWindow()
    {
        chatWindow.SetActive(false);
    }
    public void OffOutline()
    {
        outline.SetActive(false);
    }
}