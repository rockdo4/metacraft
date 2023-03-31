using System.Collections;
using UnityEngine;

public class SouthSilverNormalAttack : MonoBehaviour
{
    public Transform droneTransform;    

    public Transform[] muzzles;

    public AttackableUnit unitData;

    public float hitInterval = 0.1f;

    Coroutine droneAttack;

    public void NormalAttackOnDamage()
    {
        droneTransform.LookAt(unitData.Target.transform.position);
        droneAttack = StartCoroutine(DroneAttack());
    }
    private IEnumerator DroneAttack()
    {
        float timer = 0f;
        float duration = hitInterval * 3;

        bool[] fired = new bool[3];

        foreach (Transform muzzle in muzzles)
        {
            EffectManager.Instance.Get(EffectEnum.MuzzleFlash1, muzzle, muzzle.localRotation);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < fired.Length; i++)
            {
                if (fired[i].Equals(false) && timer > hitInterval * (i + 1))
                {
                    unitData.NormalAttackOnDamage();
                    fired[i] = true;
                }
            }

            yield return null;
        }
    }

    //private void DroneAttack()
    //{
    //    unitData.NormalAttackOnDamage();
    //}
}
