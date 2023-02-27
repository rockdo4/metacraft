using UnityEngine;

public class MissionMarkData : MonoBehaviour
{
    public bool isMarkOn;


    public void OnOffMark(bool onOff)
    {
        isMarkOn = onOff;
    }

}
