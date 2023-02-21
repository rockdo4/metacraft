using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FindTarget : MonoBehaviour
{
    private NavMeshAgent pathFind;
    private GameObject target;

    private bool isNoneEnemy = false;
    public bool IsTarget {
        get {
            if(target == null)
                return false;
            return Vector3.Distance(target.transform.position, transform.position) < 10;
        }
    }

    private void Awake()
    {
        pathFind = gameObject.GetComponent<NavMeshAgent>();
    }

    private void SetTarget()
    {
        var Enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        if (Enemys.Count == 0)
        {
            target = null;
            isNoneEnemy = true;
            return;
        }
        target = Enemys.OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }

    private void FixedUpdate()
    {
        if (!isNoneEnemy)
        {
            if (target != null)
            {
                pathFind.SetDestination(target.transform.position);
            }
        }

        if (target == null)
            SetTarget();

    }
}
