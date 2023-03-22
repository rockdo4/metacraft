using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecruitmentWindow : MonoBehaviour
{
    //public Image pageImage;
    public RecruitmentData type; //영입 타입
    private List<GameObject> heroDatabase; // 히어로 전체 데이터
    private float[] probs; // 히어로별 확률 저장
    private Dictionary<int, int> gradeCounts = new Dictionary<int, int>(); // 전체 히어로 등급별 갯수 저장
    private List<GameObject> getGacha = new(); // 영입 결과 저장 리스트
    public GameObject gachaPrefeb; //영입 결과 프리팹
    public Transform showGacha; // 영입 결과 프리팹 생성위치
    private List<RecruitmentInfo> resultRecruitmentList = new(); // 영입 결과 프리팹 생성 및 정보 불러온 리스트
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
        GameObject hero = heroDatabase[Gacha(probs)];
        GachaProcess(hero, 0);
    }

    private void GachaProcess(GameObject hero, int i)
    {
        bool isDuplicate = false;
        CharacterDataBundle herobundle = hero.GetComponent<CharacterDataBundle>();
        string heroName = herobundle.originData.name;
        var myHeroes = GameManager.Instance.myHeroes;

        // 중복 검사
        foreach (var myhero in myHeroes)
        {
            if (heroName.Equals(myhero.Value.GetComponent<CharacterDataBundle>().originData.name))
            {
                isDuplicate = true;
                break;
            }
        }

        if (!isDuplicate)
        {
            for (int j = 0; j < i; j++)
            {
                if (hero.Equals(getGacha[j]))
                {
                    isDuplicate = true;
                    break;
                }
            }
        }

        // 가챠 정보창 생성
        GameObject obj = Instantiate(gachaPrefeb, showGacha);
        RecruitmentInfo info = obj.GetComponent<RecruitmentInfo>();

        // 중복 여부에 따른 분기
        if (!isDuplicate)
        {
            // 새 Hero 생성
            GameObject newHero = GameManager.Instance.CreateNewHero(heroName);
            getGacha.Add(newHero);

            info.SetData(getGacha[i].GetComponent<CharacterDataBundle>());
        }
        else
        {
            // 중복. 임시코드. 아이템으로 대체하도록 함
            info.SetData(herobundle);
            info.icon.color = Color.gray;
        }

        resultRecruitmentList.Add(info);
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
            GachaProcess(getGacha[i], i);
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

    private void OnRateInfo()
    {
        for (int i = 0; i < heroDatabase.Count; i++)
        {
            string name = heroDatabase[i].GetComponent<CharacterDataBundle>().originData.name;
            float rate = probs[i];
            rateInfo.text += $"{GameManager.Instance.GetStringByTable(name)} : {rate:F2}%\n";
        }
    }
}
