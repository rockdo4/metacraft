using System.Collections;
using UnityEngine;

public class SouthSilverNormalAttack : MonoBehaviour
{
    public Transform droneTransform;    

    public Transform[] muzzles;

    public AttackableUnit unit;

    public float hitInterval = 0.1f;

    private Coroutine droneAttack;

    public AudioSource[] bulletAudios;

    float timerWhenMoving;
    Quaternion originRot = Quaternion.Euler(Vector3.zero);

    private void Awake()
    {
        var audioSourcesHolder = AudioManager.Instance.GetAudioResourcesHolder(transform.parent.name);
        for(int i = 0; i < bulletAudios.Length; i++)
        {
            bulletAudios[i] = Instantiate(bulletAudios[i], audioSourcesHolder);
        }        
    }

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

        droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, originRot, timerWhenMoving);
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

        if (unit.Target == null)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(unit.Target.transform.position - transform.position);

        bulletAudios[3].Play();
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
                    bulletAudios[i].Play();
                    fired[i] = true;
                }
            }

            yield return null;
        }
    }
}
