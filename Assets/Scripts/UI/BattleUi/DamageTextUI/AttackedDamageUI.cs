using UnityEngine;
using UnityEngine.Pool;
public class AttackedDamageUI : MonoBehaviour
{
    private DamageUiObjPool pools;
    private IObjectPool<DamageUI> normalPool;
    private IObjectPool<DamageUI> critPool;

    public float heightAboveGround = 2f;

    //private void Start()
    //{
    //    pools = DamageUiObjPool.Instance.GetComponent<DamageUiObjPool>();        
    //    normalPool = pools.NormalPool;
    //    critPool = pools.CritPool;        
    //}

    private void GetPool()
    {
        if (pools != null)
            return;

        pools = DamageUiObjPool.Instance.GetComponent<DamageUiObjPool>();
        normalPool = pools.NormalPool;
        critPool = pools.CritPool;
    }

    //OnAttack을 각자 원하는데로 수정. 아래는 예시.
    public void OnAttack(int damage, bool isCritical, Vector3 enemyPos, DamageType type)
    {
        GetPool();

        var dmgUI = isCritical ? critPool.Get() : normalPool.Get();
        dmgUI.SetUI(damage, enemyPos, type, heightAboveGround);
    }
}
