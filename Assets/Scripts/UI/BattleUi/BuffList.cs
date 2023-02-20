using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    // private readonly int maxViewBuffCount = 3;

    public HeroBuff buffPref;
    public GameObject buffPopUpPref;
    [SerializeField]
    public List<HeroBuff> buffList; //���� ����� ���� ����Ʈ

    [SerializeField]
    private Transform viewBuffTr; //ĳ���� ���� ����
    [SerializeField]
    private Transform popUpBuffTr; //�˾�â ����
    [SerializeField]
    public GameObject popUp;

    public void SetList(ref List<HeroBuff> heroBuffs) => this.buffList = heroBuffs;

    public void AddBuff()
    {
        var viewBuff = Instantiate(buffPref, viewBuffTr);   //ĳ���� ������ ����Ʈ�� ���� �߰��ϰ�, �˾����� �߰�
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
