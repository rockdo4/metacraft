using UnityEngine;
using UnityEngine.UI;

public class RoadChoiceButton : MonoBehaviour
{
    private Button button;
    public int choiceIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public Button GetButton()
    {
        return button;
    }
}
