using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    public BuffType buffType;
    //public float duration; //�� ���ӽð�

    public Action OnEnd;
    public GameObject popUpBuff;
    public TextMeshProUGUI count;
    public Image iconImage;

    public void EndBuffIcon()
    {
        if (OnEnd != null)
            OnEnd();

        Destroy(popUpBuff);
        Destroy(gameObject);
    }

}
