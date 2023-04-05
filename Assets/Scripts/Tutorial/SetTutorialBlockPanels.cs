using UnityEngine;
using UnityEngine.UI;

public class SetTutorialBlockPanels : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        TutorialBlockPanels.Instance.SetPanelsSurroundTarget(image);
    }
}
