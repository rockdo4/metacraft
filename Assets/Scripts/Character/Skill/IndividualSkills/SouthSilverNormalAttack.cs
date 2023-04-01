using System.Collections;
using UnityEngine;

public class SouthSilverNormalAttack : MonoBehaviour
{
    public Transform droneTransform;    

    public Transform[] muzzles;

    public AttackableUnit unitData;

    public float hitInterval = 0.1f;

    private Coroutine droneAttack;    

    public void NormalAttackOnDamage()
    {        
        droneAttack = StartCoroutine(DroneAttack());
    }
    private IEnumerator DroneAttack()
    {
        float timer = 0f;
        float duration = hitInterval * 3;
        float angleVelocity = 2f;

        bool[] fired = new bool[3];

        Quaternion targetRotation = Quaternion.LookRotation(unitData.Target.transform.position - transform.position);

        foreach (Transform muzzle in muzzles)
        {
            EffectManager.Instance.Get(EffectEnum.MuzzleFlash1, muzzle, muzzle.localRotation);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, targetRotation, timer * angleVelocity);

            for (int i = 0; i < fired.Length; i++)
            {
                if (!fired[i] && timer > hitInterval * (i + 1))
                {
                    unitData.NormalAttackOnDamage();
                    fired[i] = true;
                }
            }

            yield return null;
        }
    }
}
