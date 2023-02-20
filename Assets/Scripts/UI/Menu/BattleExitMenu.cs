using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleExitMenu : MonoBehaviour
{
    public BattleMenu battleMenu;
    public void OnClickeReturn()
    {
        Instantiate(battleMenu, transform.parent);
        Destroy(gameObject);
    }
    public void OnClickeExit()
    {
        Destroy(gameObject);
    }
}
