using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUi : MonoBehaviour
{
    public Transform herosTr;
    public StageEnemy stageEnemy;

    public BattleMenu battleMenu;

    public void OnClickMenu()
    {
        Instantiate(battleMenu, transform);
    }
}
