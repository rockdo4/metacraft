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

    private int nowLevel; //���� ����
    private int nowExp; //���� exp

    private int addLevel = 0; //���� �� ���� ���� ��
    private int lastExp = 0; //���� �� ���� exp
    private bool isMove = false;

    private Dictionary<int, int> expTable;
    private AttackableUnit thisHero;

    public void SetInfo(AttackableUnit hero)
    {
        //SetTestTable();
        expTable = GameManager.Instance.expRequirementTable;
        thisHero = hero;
        LiveData data = hero.GetUnitData().data;
        nowLevel = data.level;
        levelText.text = $"{nowLevel}";

        nowExp = data.exp;
        expImage.fillAmount = (float)nowExp / expTable[nowLevel];
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"icon_{data.name}");
    }

    public void Clear(int getExp)
    {
        expText.text = $"{getExp}";
        int nextLevel = nowLevel;
        int tempExp = nowExp;
        int needExp = expTable[nextLevel] - tempExp; //�������� �ʿ��� ����ġ

        while (true)
        {
            if (!expTable.ContainsKey(nextLevel)) //�ִ뷹�� �̸� ����ġ ���� �� ���� ��
            {
                tempExp = Mathf.Min(tempExp + getExp, expTable[nextLevel]); // ����ġ�� ����. �ִ����ġ ���� �ʰ�
                break;
            }
            else if (getExp >= needExp) //getExp ���� �������� �ʿ� ����ġ���� �����ÿ�
            {
                getExp -= needExp; //getExp ���� �ٿ��ְ�
                nextLevel++;
                needExp = expTable[nextLevel]; //�ʿ��� ����ġ�� �������� ����ġ

                tempExp = 0; //������ ������, ���� ������ �ܿ� ����ġ�� 0

                AudioManager.Instance.PlayUIAudio(3);

                thisHero.LevelupStats();
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
        thisHero.SetLevelExp(nextLevel, tempExp);

        isMove = true;
    }

    private void Update()
    {
        if (isMove)
        {
            expImage.fillAmount += Time.deltaTime;
            var nowFill = (float)lastExp / expTable[nowLevel];
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

                if (expTable.ContainsKey(textLevel))
                {
                    levelText.text = (int.Parse(levelText.text) + 1).ToString();
                    expImage.fillAmount = 0;
                }
            }
        }
    }
}
