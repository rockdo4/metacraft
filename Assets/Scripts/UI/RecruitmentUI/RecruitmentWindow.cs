using System.Collections.Generic;
using UnityEngine;

public class RecruitmentWindow : MonoBehaviour
{
    public RecruitmentData type;
    private List<GameObject> heroDatabase;
    private float[] probs;
    private Dictionary<int, int> gradeCounts = new Dictionary<int, int>();
    private List<GameObject> getGacha = new ();
    public GameObject gachaPrefeb;
    public Transform showGacha;
    private List<RecruitmentInfo> resultRecruitmentList = new ();

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

    public void OneTimeGacha()
    {
        getGacha.Add(heroDatabase[Gacha(probs)]);

        GameObject obj = Instantiate(gachaPrefeb, showGacha);
        RecruitmentInfo info = obj.GetComponent<RecruitmentInfo>();
        info.SetData(getGacha[0].GetComponent<CharacterDataBundle>());
        resultRecruitmentList.Add(info);
    }
    public void TenTimesGacha()
    {
        for (int i = 0; i < 10; i++)
        {
            getGacha.Add(heroDatabase[Gacha(probs)]);
        }
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(gachaPrefeb, showGacha);
            RecruitmentInfo info = obj.GetComponent<RecruitmentInfo>();
            info.SetData(getGacha[i].GetComponent<CharacterDataBundle>());
            resultRecruitmentList.Add(info);
        }
    }

    private int Gacha(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
