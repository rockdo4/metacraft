using UnityEngine;

public class BattleUi : MonoBehaviour
{
    public Transform herosTr;
    public StageEnemy stageEnemy;
    public BattleMenu battleMenu;

    public void SetEnemyCount(int c) => stageEnemy.Count = c;
    public void Hero(BattleHero hero) => Instantiate(hero, herosTr);

    public void OnClickMenu()
    {
        Instantiate(battleMenu, transform);
    }


}
