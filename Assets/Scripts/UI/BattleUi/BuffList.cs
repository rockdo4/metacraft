using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    // private readonly int maxViewBuffCount = 3;

    public HeroBuff buffPref;
    public GameObject buffPopUpPref;
    [SerializeField]
    public List<HeroBuff> buffList; //현재 적용된 버프 리스트

    [SerializeField]
    private Transform viewBuffTr; //캐릭터 옆에 버프
    [SerializeField]
    private Transform popUpBuffTr; //팝업창 버프
    [SerializeField]
    public GameObject popUp;

    public void SetList(ref List<HeroBuff> heroBuffs) => this.buffList = heroBuffs;

    public void AddBuff()
    {
        var viewBuff = Instantiate(buffPref, viewBuffTr);   //캐릭터 좌측의 리스트에 버프 추가하고, 팝업에도 추가
        var popUpBuff = Instantiate(buffPopUpPref, popUpBuffTr);

        viewBuff.popUpBuff = popUpBuff;
        viewBuff.duration = 3f;

        buffList.Add(viewBuff); 

        viewBuff.OnEnd = () => buffList.Remove(viewBuff);
    }

    public void OnClickPopUp()
    {
        if(!popUp.activeSelf)
            popUp.SetActive(true);
    }
}
