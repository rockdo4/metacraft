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

        GameObject newHero = gm.CreateNewHero(count);
        //gm.myHeroes.Add(newHero);
        //newHero.SetActive(false);

        LiveData data = newHero.GetComponent<AttackableUnit>().GetUnitData().data;
        portrait.sprite = gm.GetSpriteByAddress($"Illu_{data.name}");
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"����� �� : {gm.GetStringByTable(data.name)}\n");
        stringBuilder.Append($"���ݷ� : {data.baseDamage}\n");
        stringBuilder.Append($"���� : {data.baseDefense}\n");
        stringBuilder.Append($"ü�� : {data.healthPoint}\n");
        stringBuilder.Append($"Ÿ�� : {GameManager.Instance.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n"); 
        stringBuilder.Append($"ġ��Ÿ Ȯ�� : {data.critical * 100:.0}%\n");
        stringBuilder.Append($"ġ��Ÿ ���� : {data.criticalDmg * 100:00.0}%\n");
        stringBuilder.Append($"�̵� �ӵ� : {data.moveSpeed}\n");
        explainText.text = stringBuilder.ToString();
    }

    private void OnDisable()
    {
        portrait.sprite = null;
        explainText.text = null;
    }
}