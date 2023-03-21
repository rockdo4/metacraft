using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentWindow : MonoBehaviour
{
    //public Image pageImage;
    public RecruitmentData type; //���� Ÿ��
    private List<GameObject> heroDatabase; // ����� ��ü ������
    private float[] probs; // ����κ� Ȯ�� ����
    private Dictionary<int, int> gradeCounts = new Dictionary<int, int>(); // ��ü ����� ��޺� ���� ����
    private List<GameObject> getGacha = new(); // ���� ��� ���� ����Ʈ
    public GameObject gachaPrefeb; //���� ��� ������
    public Transform showGacha; // ���� ��� ������ ������ġ
    private List<RecruitmentInfo> resultRecruitmentList = new(); // ���� ��� ������ ���� �� ���� �ҷ��� ����Ʈ
    public TextMeshProUGUI rateInfo;

    private void Start()
    {
        //pageImage.sprite = GameManager.Instance.GetSpriteByAddress($"{type.image}");
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

        OnRateInfo();
    }

    public void OneTimeGacha()
    {
        ClearResult();

        bool isDuplicate = false;
        var hero = heroDatabase[Gacha(probs)];
        string heroName = hero.GetComponent<CharacterDataBundle>().originData.name;
        var myHeroes = GameManager.Instance.myHeroes;
        foreach (var myhero in myHeroes)
        {
            if (heroName.Equals(myhero.Value.GetComponent<CharacterDataBundle>().originData.name))
            {
                Logger.Debug("�ߺ�");
                isDuplicate = true;
                break;
            }
        }

        GameObject obj = Instantiate(gachaPrefeb, showGacha);
        RecruitmentInfo info = obj.GetComponent<RecruitmentInfo>();
        if (!isDuplicate)
        {
            GameManager.Instance.CreateNewHero(heroName);
            getGacha.Add(hero);

            info.SetData(getGacha[0].GetComponent<CharacterDataBundle>());
            resultRecruitmentList.Add(info);
        }
        else
        {

            resultRecruitmentList.Add(info);
        }

    }
    public void TenTimesGacha()
    {
        ClearResult();
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

    private void ClearResult()
    {
        getGacha.Clear();
        foreach (var result in resultRecruitmentList)
        {
            Destroy(result.gameObject);
        }
        resultRecruitmentList.Clear();
    }

    private void DuplicateInspection()
    {

    }

    private void OnRateInfo()
    {
        for (int i = 0; i < heroDatabase.Count; i++)
        {
            string name = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            float rate = probs[i];
            rateInfo.text += $"{GameManager.Instance.GetStringByTable(name)} : {rate.ToString("F2")}%\n";
        }
    }
}
