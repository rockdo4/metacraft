using System.Collections;
using UnityEngine;

public class SouthSilverNormalAttack : MonoBehaviour
{
    public Transform droneTransform;    

    public Transform[] muzzles;

    public AttackableUnit unit;

    public float hitInterval = 0.1f;

    private Coroutine droneAttack;

    float timerWhenMoving;
    Quaternion worldForward = Quaternion.LookRotation(Vector3.forward);

    private void Update()
    {
        if (unit.GetUnitState() != UnitState.MoveNext)
        {
            timerWhenMoving = 0f;
            return;
        }

        timerWhenMoving += Time.deltaTime;

        if (timerWhenMoving > 1f)
            return;

        droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, worldForward, timerWhenMoving);
    }

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

        Quaternion targetRotation = Quaternion.LookRotation(unit.Target.transform.position - transform.position);

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
                    unit.NormalAttackOnDamage();
                    fired[i] = true;
                }
            }

            yield return null;
        }
    }
}
