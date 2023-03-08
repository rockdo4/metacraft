using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelectUiActiveFalse : MonoBehaviour
{
    public List<GameObject> uiList = new List<GameObject>();

    public void ActiveFalseUiList()
    {
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].SetActive(false);
        }
    }
}
