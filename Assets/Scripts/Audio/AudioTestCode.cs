using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestCode : MonoBehaviour
{
    void Update()
    {
        Test();
    }
    private void Test()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SoundManager.PlaySE(SeList.Test1);            
        }
    }
}
