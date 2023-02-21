using UnityEngine;

public class BattleUi : MonoBehaviour
{
    public Transform herosTr;
    public StageEnemy stageEnemy;
    public BattleMenu battleMenu;
    public Transform popUp;

    public void SetEnemyCount(int c) => stageEnemy.Count = c;


    public void OnClickMenu()
    {
        Instantiate(battleMenu, transform);
    }


}
