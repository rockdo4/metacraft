using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearHeroInfo : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI getExp;
    public Image exp;

    private int nowLevel; //현재 레벨
    private int nowExp; //현재 exp

    private int addLevel = 0; //연산 후 레벨 증가 값
    private int lastExp = 0; //연산 후 남은 exp
    private bool isMove = false;

    private Dictionary<int, int> ExpTable = new();

    private void Awake()
    {
        SetTest();
        Init(1, 0);
    }

    public void Init(int level, int exp)
    {
        nowLevel = level;
        nowExp = exp;
    }

    public void SetTest()
    {
        ExpTable[1] = 100;
        ExpTable[2] = 200;
        ExpTable[3] = 300;
        ExpTable[4] = 400;
        ExpTable[5] = 500;
        ExpTable[6] = 600;
    }

    public void Clear(int getExp)
    {
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
                tempExp = 0; //레벨업 했으니, 현재 레벨의 잔여 경험치는 0ㄴ
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

        isMove = true;
    }

    public int testGetExp;
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Clear(testGetExp);
        //}

        if (isMove)
        {
            exp.fillAmount += Time.deltaTime;
            var nowFill = ((float)lastExp / (float)ExpTable[nowLevel]);
            if (addLevel == 0 && exp.fillAmount >= nowFill)
            {
                addLevel = 0;
                lastExp = 0;

                exp.fillAmount = nowFill;
                isMove = false;
            }
            else if (exp.fillAmount >= 1)
            {
                addLevel--;

                var textLevel = (int.Parse(level.text) + 1);

                if (ExpTable.ContainsKey(textLevel))
                {
                    level.text = (int.Parse(level.text) + 1).ToString();
                    exp.fillAmount = 0;
                }
            }
        }
    }
}
