using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }
    public GameObject popupHolder;
    private void Awake()
    {
        Instance = this;
    }

}
