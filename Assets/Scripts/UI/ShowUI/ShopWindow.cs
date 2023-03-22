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
        // 임시 코드
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
        stringBuilder.Append($"히어로 명 : {gm.GetStringByTable(data.name)}\n");
        stringBuilder.Append($"공격력 : {data.baseDamage}\n");
        stringBuilder.Append($"방어력 : {data.baseDefense}\n");
        stringBuilder.Append($"체력 : {data.healthPoint}\n");
        stringBuilder.Append($"타입 : {GameManager.Instance.GetStringByTable($"herotype_{(CharacterJob)data.job}")}\n"); 
        stringBuilder.Append($"치명타 확률 : {data.critical * 100:.0}%\n");
        stringBuilder.Append($"치명타 배율 : {data.criticalDmg * 100:00.0}%\n");
        stringBuilder.Append($"이동 속도 : {data.moveSpeed}\n");
        explainText.text = stringBuilder.ToString();
    }

    private void OnDisable()
    {
        portrait.sprite = null;
        explainText.text = null;
    }
}