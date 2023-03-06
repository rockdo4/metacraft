using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearHeroInfo : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public Image expImage;

    private int nowLevel; //���� ����
    private int nowExp; //���� exp

    private int addLevel = 0; //���� �� ���� ���� ��
    private int lastExp = 0; //���� �� ���� exp
    private bool isMove = false;

    private Dictionary<int, int> ExpTable = new();

    public void SetInfo(int level, int exp)
    {
        SetTest();
        nowLevel = level;
        levelText.text = $"{nowLevel}";

        nowExp = exp;
        expImage.fillAmount = (float)nowExp / ExpTable[nowLevel];
    }

    public void SetTest()
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
        if (isMove)
        {
            expImage.fillAmount += Time.deltaTime;
            var nowFill = ((float)lastExp / (float)ExpTable[nowLevel]);
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
