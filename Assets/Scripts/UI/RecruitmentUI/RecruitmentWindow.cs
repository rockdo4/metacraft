using System.Collections.Generic;
using System.Text;
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
    private bool ratePopup = false;

    public List<Dictionary<string, object>> itemInfoList; // 아이템 정보

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

        itemInfoList = GameManager.Instance.itemInfoList;
        ratePopup = false;
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

            try
            {
                string stoneId = GameManager.Instance.recruitmentReplacementTable.
                Find(t => t["Name"].ToString().CompareTo(herobundle.originData.name) == 0)["Replacement"].ToString();

                info.SetStoneData((CharacterGrade)herobundle.originData.grade,
                    itemInfoList.Find(t => t["ID"].ToString().CompareTo(stoneId) == 0));
                GameManager.Instance.inventoryData.AddItem(stoneId);
            }
            catch
            {
                Logger.Debug("this");
            }
            //info.icon.color = Color.gray;
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
        List<string> gradeCList = new ();
        List<string> gradeBList = new ();
        List<string> gradeAList = new ();

        for (int i = 0; i < heroDatabase.Count; i++)
        {
            CharacterDataBundle cdb = heroDatabase[i].GetComponent<CharacterDataBundle>();
            string name = cdb.originData.name;
            CharacterGrade grade = (CharacterGrade)cdb.originData.grade;
            float rate = probs[i];
            string result = $"{GameManager.Instance.GetStringByTable(name)} : {rate:F2}%";
            switch (grade)
            {
                case CharacterGrade.C:
                    gradeCList.Add(result);
                    break;
                case CharacterGrade.B:
                    gradeBList.Add(result);
                    break;
                default:
                    gradeAList.Add(result);
                    break;
            }
            //rateInfo.text += $"{GameManager.Instance.GetStringByTable(name)} : {rate:F2}%\n";
        }

        StringBuilder sb = new ();

        sb.AppendLine("A 등급");
        int count = gradeAList.Count;
        for (int i = 0; i < count; i++)
        {
            sb.AppendLine(gradeAList[i]);
        }

        sb.AppendLine("\nB 등급");
        count = gradeBList.Count;
        for (int i = 0; i < count; i++)
        {
            sb.AppendLine(gradeBList[i]);
        }

        sb.AppendLine("\nC 등급");
        count = gradeCList.Count;
        for (int i = 0; i < count; i++)
        {
            sb.AppendLine(gradeCList[i]);
        }

        rateInfo.text = sb.ToString();
    }

    public void SetRatePopup()
    {
        SetRatePopupMode(!ratePopup);
    }

    private void SetRatePopupMode(bool value)
    {
        ratePopup = value;
        if (!ratePopup)
            UIManager.Instance.ClearPopups();
    }

    private void OnDisable()
    {
        SetRatePopupMode(false);
    }
}
