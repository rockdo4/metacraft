using TMPro;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    private void Awake()
    {
        textBox = GetComponentInChildren<TextMeshProUGUI>();
    }
}
