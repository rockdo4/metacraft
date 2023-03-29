using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject outline;
    public GameObject chatWindow;
    public TextMeshProUGUI textObject;

    public bool isMask = false;

    public void SetText(string txt)
    {
        textObject.text = txt;
    }
    public void OnWindow()
    {
        Logger.Debug(chatWindow.name);
        chatWindow.SetActive(true);
    }
    public void OnOutline()
    {
        if (outline != null)
            outline.SetActive(true);
    }
    public void OffWindow()
    {
        chatWindow.SetActive(false);
    }
    public void OffOutline()
    {
        if (outline != null)
            outline.SetActive(false);
    }
}
