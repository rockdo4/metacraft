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
