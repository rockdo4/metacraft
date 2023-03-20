using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitmentWindow : MonoBehaviour
{
    public RecruitmentData type;
    public List<GameObject> heroDatabase;
    private float[] probs;
    private Dictionary<int, int> gradeCounts = new Dictionary<int, int>();

    private void Start()
    {
        heroDatabase = GameManager.Instance.heroDatabase;
        probs = new float[heroDatabase.Count];
        gradeCounts.Add(1, 0);
        gradeCounts.Add(2, 0);
        gradeCounts.Add(3, 0);

        foreach (var hero in heroDatabase)
        {

            int grade = hero.GetComponent<CharacterDataBundle>().originData.grade;

            if (gradeCounts.ContainsKey(grade))
            {
                gradeCounts[grade]++;
            }
        }

        for (int i = 0; i < heroDatabase.Count; i++)
        {
            switch (heroDatabase[i].GetComponent<CharacterDataBundle>().originData.grade)
            {
                case 1:
                    {
                        probs[i] = type.cGradeRate / gradeCounts[1];
                        break;
                    }
                case 2:
                    {
                        probs[i] = type.bGradeRate / gradeCounts[2];
                        break;
                    }
                case 3:
                    {
                        probs[i] = type.aGradeRate / gradeCounts[3];
                        break;
                    }
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {

        }
    }
}
