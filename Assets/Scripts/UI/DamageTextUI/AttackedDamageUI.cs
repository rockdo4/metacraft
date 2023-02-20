using UnityEngine;
using UnityEngine.Pool;
public class AttackedDamageUI : MonoBehaviour
{
    private GameObject damageUI;    
    private DamageUiObjPool pools;
    private IObjectPool<DamageUI> normalPool;
    private IObjectPool<DamageUI> critPool;    

    private void Start()
    {
        damageUI = DamageUiObjPool.Instance.gameObject;
        pools = damageUI.GetComponent<DamageUiObjPool>();
        normalPool = pools.NormalPool;
        critPool = pools.CritPool;
    }
    public void OnAttack(int damage, bool isCritical, Vector3 enemyPos)
    {
        var dmgUI = isCritical ? critPool.Get() : normalPool.Get();
        dmgUI.SetUI(damage, enemyPos);
    }
}
