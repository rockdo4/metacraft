using UnityEngine;
using UnityEngine.Pool;
public class AttackedDamageUI : MonoBehaviour
{   
    private IObjectPool<DamageUI> normalPool;
    private IObjectPool<DamageUI> critPool;    

    private void Start()
    {        
        var pools = DamageUiObjPool.Instance.GetComponent<DamageUiObjPool>();        
        normalPool = pools.NormalPool;
        critPool = pools.CritPool;
    }
    public void OnAttack(int damage, bool isCritical, Vector3 enemyPos)
    {
        var dmgUI = isCritical ? critPool.Get() : normalPool.Get();
        dmgUI.SetUI(damage, enemyPos);
    }
}
