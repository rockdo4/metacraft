using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearHeroInfo : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI getExp;
    public Image exp;

    private int nowLevel; //���� ����
    private int nowExp; //���� exp

    private int addLevel = 0; //���� �� ���� ���� ��
    private int lastExp = 0; //���� �� ���� exp
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
        int needExp = ExpTable[nextLevel] - tempExp; //�������� �ʿ��� ����ġ

        while (true)
        {
            if (!ExpTable.ContainsKey(nextLevel + 1)) //�ִ뷹�� �̸� ����ġ ���� �� ���� ��
            {
                tempExp = Mathf.Min(tempExp + getExp, ExpTable[nextLevel]); // ����ġ�� ����. �ִ����ġ ���� �ʰ�
                break;
            }
            else if (getExp >= needExp) //getExp ���� �������� �ʿ� ����ġ���� �����ÿ�
            {
                getExp -= needExp; //getExp ���� �ٿ��ְ�
                needExp = ExpTable[nextLevel + 1]; //�ʿ��� ����ġ�� �������� ����ġ

                nextLevel++;
                tempExp = 0; //������ ������, ���� ������ �ܿ� ����ġ�� 0��
            }
            else //������ ���ϴ� ���¸� ���� ��
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
