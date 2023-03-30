using TMPro;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject outline;
    public GameObject chatWindow;
    public TextMeshProUGUI textObject;
    public TutorialMask tutorialMask;

    public bool isMask = false;

    public void SetText(string txt)
    {
        textObject.text = txt;
    }
    public void OnWindow()
    {
        if (chatWindow != null)
            chatWindow.SetActive(true);
    }
    public void OnOutline()
    {
        if (outline != null)
        {
            outline.SetActive(true);
        }
    }
    public void OffWindow()
    {
        if (chatWindow != null)
            chatWindow.SetActive(false);
    }
    public void OffOutline()
    {
        if (outline != null)
            outline.SetActive(false);
    }
}
