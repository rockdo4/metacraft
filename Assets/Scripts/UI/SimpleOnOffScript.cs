using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOnOffScript : MonoBehaviour
{
    public void SetOnOff(bool toggle)
    {
        gameObject.SetActive(toggle);
    } 
}
