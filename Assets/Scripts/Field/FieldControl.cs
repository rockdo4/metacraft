using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldControl : MonoBehaviour
{
    public GameObject[] offices;

    private void Start()
    {
        GameManager.Instance.playerLevelUp += OfficeChange;
    }

    private void OfficeChange(string objName)
    {
        for (int i = 0; i < offices.Length; i++)
        {
            offices[i].SetActive(false);
            if(offices[i].name.Equals(objName))
            {
                offices[i].SetActive(true);
            }
        }
    }
}
