using UnityEngine;

public class UItestCode : MonoBehaviour
{
    private AttackedDamageUI attacked;
    private void Start()
    {
        attacked = GetComponent<AttackedDamageUI>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            attacked.OnAttack(10, false, transform.position);
        }
    }
}
