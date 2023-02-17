using UnityEngine;
using UnityEngine.UI;
public class View : MonoBehaviour
{   
    //public virtual void Init() => button?.onClick.AddListener(() => ViewManager.Show(index));
    public virtual void Hide() => gameObject.SetActive(false);
    public virtual void Show() => gameObject.SetActive(true);
}
