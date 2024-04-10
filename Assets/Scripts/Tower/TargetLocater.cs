using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocater : MonoBehaviour
{
    GameObject target;
    Transform weapon;
    ParticleSystem ps;
    bool targetLost = true;

    [SerializeField] float range = 15f;

    void Awake()
    {
        target = GameObject.Find("Enemy");
        weapon = transform.Find("BallistaTopMesh");
        ps = weapon.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
        if (targetLost)
        {
            Attack(false);
            FindClosestTarget();
        }
        else
        {
            Attack(true);
        }

        AimWeapon();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies)
        {
            // Use SqrMagnitude to estimate the distance between two points, it's faster than Distance
            float distanceToEnemy = Vector3.SqrMagnitude(transform.position - enemy.transform.position);

            if (distanceToEnemy < maxDistance)
            {
                maxDistance = distanceToEnemy;
                closestTarget = enemy.transform;
            }
        }
        if (closestTarget)
        {
            target = closestTarget.gameObject;
            targetLost = false;
        }
    }

    void AimWeapon()
    {
        if (!target) return;  // if target is null, return
        weapon.LookAt(target.transform);
    }

    void Attack(bool isActive)
    {
        var emissionModule = ps.emission;
        emissionModule.enabled = isActive;
    }

    void CheckTarget()
    {
        if (!target || target.activeSelf == false)  // since Object Pooling is used, check if the target is active
        {
            targetLost = true;
        }
        else
        {
            // Debug.Log(Vector3.SqrMagnitude(transform.position - target.transform.position));
            targetLost = Vector3.SqrMagnitude(transform.position - target.transform.position) > range * range;
            // Debug.Log("targetLost: "+targetLost);
        }

    }
}
