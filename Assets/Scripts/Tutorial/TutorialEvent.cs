using TMPro;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    private void Start()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }
}
