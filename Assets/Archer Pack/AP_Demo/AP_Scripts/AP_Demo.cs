using UnityEngine;
using UnityEngine.UI;

public class AP_Demo : MonoBehaviour
{
    public GameObject[] ProjectilesList;

    public Text Title;
    public int Selection = 0;

    public GameObject BackText;
    public GameObject NextText;
    public GameObject BackButton;
    public GameObject NextButton;


    void Start()
    {
        ProjectilesList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + ProjectilesList[Selection].gameObject.transform.name.ToString();
    }
    void Update()
    {

        if (Selection == 0)
        {
            BackText.SetActive(false);
            BackButton.SetActive(false);
        }
        else
        {
            BackText.SetActive(true);
            BackButton.SetActive(true);
        }
        if (Selection == ProjectilesList.Length - 1)
        {
            NextText.SetActive(false);
            NextButton.SetActive(false);
        }
        else
        {
            NextText.SetActive(true);
            NextButton.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Next();
        }


    }
    public void Back()
    {

        if (Selection < ProjectilesList.Length && Selection != 0)
        {
            ProjectilesList[Selection].SetActive(false);
            Selection -= 1;
            ProjectilesList[Selection].SetActive(true);
            Title.text = "Prefab Name: " + ProjectilesList[Selection].gameObject.transform.name.ToString();
        }

    }

    public void Next()
    {
        if (Selection < ProjectilesList.Length && Selection != ProjectilesList.Length - 1)
        {
            ProjectilesList[Selection].SetActive(false);
            Selection += 1;
            ProjectilesList[Selection].SetActive(true);
            Title.text = "Prefab Name: " + ProjectilesList[Selection].gameObject.transform.name.ToString();
        }
    }

    public void Last()
    {
        if (Selection < ProjectilesList.Length)
        {
            ProjectilesList[Selection].SetActive(false);
        }

    }
}
