using UnityEngine;

public class UItestCode : MonoBehaviour
{
    private AttackedDamageUI attacked;
    private HpBarManager hpBarManager;
    private void Start()
    {
        attacked = GetComponent<AttackedDamageUI>();
        hpBarManager = GetComponent<HpBarManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            attacked.OnAttack(10, false, transform.position, DamageType.Normal);
            //hpBarManager.OnDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            attacked.OnAttack(10, false, transform.position, DamageType.Heal);
            //hpBarManager.OnDamage( - 10f);
        }
    }
}