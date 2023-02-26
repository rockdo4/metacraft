using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : View
{
    public Image portrait;
    public TextMeshProUGUI explainText;

    public void OnClickCraft()
    {
        // �ӽ� �ڵ�
        GameManager gm = GameManager.Instance;
        int count = gm.myHeroes.Count;
        if (count == gm.heroDatabase.Count)
            return;

        GameObject newHero = Instantiate(gm.heroDatabase[count], gm.heroSpawnTransform);
        gm.myHeroes.Add(newHero);
        newHero.SetActive(false);

        LiveData data = newHero.GetComponent<AttackableUnit>().GetHeroData().data;
        portrait.sprite = gm.GetSpriteByAddress($"Illur_{data.name}");
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {data.name}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {data.job}\n");
        stringBuilder.Append($"Ȱ���� �Ҹ� : {data.energy}\n");
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical}\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg}\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        stringBuilder.Append($"���߷� : {data.accuracy}\n");
        stringBuilder.Append($"ȸ���� : {data.evasion}\n");
        explainText.text = stringBuilder.ToString();
    }

    private void OnDisable()
    {
        portrait.sprite = null;
        explainText.text = null;
    }
}