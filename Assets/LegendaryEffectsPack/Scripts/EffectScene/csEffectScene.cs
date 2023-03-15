using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csEffectScene : MonoBehaviour {

    public Transform[] m_effects;
    public Text m_packageName;
    public Text m_effectName;
    public Text m_version;
    public Text m_explain;

    public static GameObject[] m_destroyObjects = new GameObject[30];
    public static int inputLocation;
    int index = 0;

    void Awake()
    {
        m_effectName.text = "0 : Effect_00_NULL";
        m_version.text = "Version : 5.0  l  'Post Processing Stack' used";
        m_explain.text = "'Z' is previous effect, 'X' is next effect, 'C' is current effect\n";
        m_explain.text += "'W' is enlarge camera distance, 'S' is shrink camera distance";

        inputLocation = 0;
        m_effectName.text = m_effects[index].name.ToString();
        MakeObject();

    }

	void Update ()
    {
        InputKey();
	}

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (index <= 0)
                index = m_effects.Length - 1;
            else
                index--;

            MakeObject();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (index >= m_effects.Length-1)
                index = 0;
            else
                index++;

            MakeObject();
        }

        if (Input.GetKeyDown(KeyCode.C))
            MakeObject();
    }

    void MakeObject()
    {
        DestroyGameObject();
        GameObject gm = Instantiate(m_effects[index],
            m_effects[index].transform.position,
            m_effects[index].transform.rotation).gameObject;
        m_effectName.text = (index+1) +" : "+m_effects[index].name.ToString();

        m_destroyObjects[inputLocation] = gm;

        inputLocation++;
    }

    void DestroyGameObject()
    {
        for(int i = 0; i < inputLocation; i++)
        {
            Destroy(m_destroyObjects[i]);
        }
        inputLocation = 0;
    }
}
