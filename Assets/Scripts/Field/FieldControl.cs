using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldControl : MonoBehaviour
{
    public GameObject[] offices;

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene OfficeScene, LoadSceneMode mode)
    {
        OfficeChange();
    }

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
