using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHero : MonoBehaviour
{
    public HeroSkill heroSkill;

    [SerializeField]
    private BuffList viewBuffList;
    public List<HeroBuff> buffList = new List<HeroBuff>();

    private void Awake()
    {
        viewBuffList.SetList(ref buffList);
    }

    [ContextMenu("Test/AddBuff")]
    public void AddBuff()
    {
        viewBuffList.AddBuff();
    }

}
