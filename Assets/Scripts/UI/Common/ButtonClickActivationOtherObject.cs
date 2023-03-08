using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickActivationOtherObject : MonoBehaviour
{
    public void OnObjectsActive(GameObject activationObject)
    {
        activationObject.SetActive(true);
    }
    public void OffObjectsActive(GameObject activationObject)
    {
        activationObject.SetActive(false);
    }
    public void ReverseObjectsActive(GameObject activationObject)
    {
        activationObject.SetActive(!activationObject.activeSelf);
    }
}
