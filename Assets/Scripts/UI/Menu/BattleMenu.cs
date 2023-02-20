using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public BattleExitMenu battleExit;
    public void OnClickOption()
    {
        Destroy(gameObject);
    }
    public void OnClickeReturn()
    {
        Destroy(gameObject);
    }
    public void OnClickeExit()
    {
        Instantiate(battleExit,transform.parent);
        Destroy(gameObject);
    }
}
