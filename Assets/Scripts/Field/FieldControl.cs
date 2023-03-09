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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < offices.Length; i++)
        {
            offices[i].SetActive(false);
            if (offices[i].name.Equals(GameManager.Instance.playerData.officeImage))
            {
                offices[i].SetActive(true);
            }
        }
    }
}
