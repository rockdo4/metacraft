using UnityEngine.UI;

public class HeroInfoView : View
{
    private Button button;
    public int index;
    private void Awake()
    {
        button = GetComponent<Button>();        
        button.onClick.AddListener(() => UIManager.Instance.ShowView(index));        
    }
}
