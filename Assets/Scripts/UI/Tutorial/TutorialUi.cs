using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUi : MonoBehaviour
{
   public List<GameObject> pages;
    int idx;
    private void Awake()
    {
        idx = 0;
    }
    public void OnLeft()
    {
        pages[idx].SetActive(false);
        idx = (idx + 1 + pages.Count) % pages.Count;
        pages[idx].SetActive(true);
    }
    public void OnRight()
    {
        pages[idx].SetActive(false);
        idx = (idx - 1 + pages.Count) % pages.Count;
        pages[idx].SetActive(true);
    }
}
