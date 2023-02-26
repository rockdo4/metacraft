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

        GameObject newHero = Instantiate(gm.heroDatabase[count], gm.heroSpawnTransform);
        gm.myHeroes.Add(newHero);
        newHero.SetActive(false);

        LiveData data = newHero.GetComponent<AttackableUnit>().GetHeroData().data;
        portrait.sprite = gm.GetSpriteByAddress($"Illur_{data.name}");
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"히어로 명 : {data.name}\n");
        stringBuilder.Append($"공격력 : {data.baseDamage}\n");
        stringBuilder.Append($"방어력 : {data.baseDefense}\n");
        stringBuilder.Append($"체력 : {data.healthPoint}\n");
        stringBuilder.Append($"타입 : {data.job}\n");
        stringBuilder.Append($"활동력 소모량 : {data.energy}\n");
        stringBuilder.Append($"치명타 확률 : {data.critical}\n");
        stringBuilder.Append($"치명타 배율 : {data.criticalDmg}\n");
        stringBuilder.Append($"이동 속도 : {data.moveSpeed}\n");
        stringBuilder.Append($"명중률 : {data.accuracy}\n");
        stringBuilder.Append($"회피율 : {data.evasion}\n");
        explainText.text = stringBuilder.ToString();
    }

    private void OnDisable()
    {
        portrait.sprite = null;
        explainText.text = null;
    }
}