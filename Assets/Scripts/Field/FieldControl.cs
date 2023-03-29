using UnityEngine;

public class FieldControl : MonoBehaviour
{
    public GameObject[] offices;

    public void Start()
    {
        for (int i = 0; i < offices.Length; i++)
        {
            offices[i].SetActive(false);
            if (offices[i].name.Equals(GameManager.Instance.playerData.officeImage))
            {
                offices[i].SetActive(true);
                break;
            }
        }
    }
}
