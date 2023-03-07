using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearHeroInfo : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public Image expImage;
    public Image heroImage;

    private int nowLevel; //현재 레벨
    private int nowExp; //현재 exp

    private int addLevel = 0; //연산 후 레벨 증가 값
    private int lastExp = 0; //연산 후 남은 exp
    private bool isMove = false;

    private Dictionary<int, int> ExpTable = new();
    private AttackableHero thisHero;

    public void SetInfo(AttackableHero hero)
    {
        SetTestTable();
        thisHero = hero;
        LiveData data = hero.GetUnitData().data;
        nowLevel = data.level;
        levelText.text = $"{nowLevel}";

        nowExp = data.exp;
        expImage.fillAmount = (float)nowExp / ExpTable[nowLevel];
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{data.name}");
    }

    public void SetTestTable()
    {
        for (int i = 1; i <= 20; i++)
        {
            ExpTable[i] = 100 * i;
        }
    }

    public void Clear(int getExp)
    {
        expText.text = $"{getExp}";
        int nextLevel = nowLevel;
        int tempExp = nowExp;
        int needExp = ExpTable[nextLevel] - tempExp; //레벨업에 필요한 경험치

        while (true)
        {
            if (!ExpTable.ContainsKey(nextLevel + 1)) //최대레벨 이면 경험치 세팅 후 연산 끝
            {
                tempExp = Mathf.Min(tempExp + getExp, ExpTable[nextLevel]); // 경험치만 증가. 최대경험치 넘지 않게
                break;
            }
            else if (getExp >= needExp) //getExp 값이 레벨업에 필요 경험치보다 많을시에
            {
                getExp -= needExp; //getExp 값을 줄여주고
                needExp = ExpTable[nextLevel + 1]; //필요한 경험치는 다음레벨 경험치

                nextLevel++;
                tempExp = 0; //레벨업 했으니, 현재 레벨의 잔여 경험치는 0
                
                // 레벨업시 합연산으로 올릴것과 곱연산으로 올릴 함수. 수정해서 사용
                //thisHero.LevelUpAdditional(10, 1, 50);
                thisHero.LevelUpMultiplication(0.1f, 0.1f, 0.1f);
            }
            else //레벨업 못하는 상태면 연산 끝
            {
                tempExp += getExp;
                break;
            }
        }

        addLevel = nextLevel - nowLevel;
        lastExp = tempExp;

        nowLevel = nextLevel;
        nowExp = tempExp;
        thisHero.SetLevelExp(nextLevel, tempExp);

        isMove = true;
    }

    private void Update()
    {
        if (isMove)
        {
            expImage.fillAmount += Time.deltaTime;
            var nowFill = (float)lastExp / ExpTable[nowLevel];
            if (addLevel == 0 && expImage.fillAmount >= nowFill)
            {
                addLevel = 0;
                lastExp = 0;

                expImage.fillAmount = nowFill;
                isMove = false;
            }
            else if (expImage.fillAmount >= 1)
            {
                addLevel--;

                var textLevel = (int.Parse(levelText.text) + 1);

                if (ExpTable.ContainsKey(textLevel))
                {
                    levelText.text = (int.Parse(levelText.text) + 1).ToString();
                    expImage.fillAmount = 0;
                }
            }
        }
    }
}
