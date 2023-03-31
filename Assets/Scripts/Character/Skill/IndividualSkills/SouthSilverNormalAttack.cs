using UnityEngine;

public class SouthSilverNormalAttack : MonoBehaviour
{
    public Transform droneTransform;
    public Transform muzzle1;
    public Transform muzzle2;
    public Transform muzzle3;
    public Transform muzzle4;

    public AttackableUnit unitData;

    public void NormalAttackOnDamage()
    {
        droneTransform.LookAt(unitData.Target.transform.position);
    }

    private void DroneAttack()
    {
        unitData.NormalAttackOnDamage();
    }
}
