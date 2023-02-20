using UnityEngine;

public class ClearUi : MonoBehaviour
{
    public Transform heroTr;
    public Rewards rewards;
    
    private int reawrdCount = 5;
    private ClearHeroInfo[] heros;

    private void Awake()
    {
        heros = heroTr.GetComponentsInChildren<ClearHeroInfo>();
        foreach (var hero in heros)
        {
            hero.SetInfo(1, 0); //실행될때 히어로 정보(레벨, 경험치 등 가져오기)
        }

        Clear();
    }
    public void Clear()
    {
        foreach (var hero in heros)
        {
            hero.Clear(400);
        }

        rewards.SetReawrd(reawrdCount);
    }
}
