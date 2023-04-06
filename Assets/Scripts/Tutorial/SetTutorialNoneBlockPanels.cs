using UnityEngine;
using UnityEngine.UI;

public class SetTutorialNoneBlockPanels : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        TutorialNoneBlockPanels.Instance.SetPanelsSurroundTarget(image);
    }

    private void OnDisable()
    {
        TutorialNoneBlockPanels.Instance.gameObject.SetActive(false);
    }
}
