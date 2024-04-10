using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocater : MonoBehaviour
{
    GameObject target;
    Transform weapon;
    ParticleSystem particleSystem;
    void Awake()
    {
        target = GameObject.Find("Enemy");
        weapon = transform.Find("BallistaTopMesh");
        particleSystem = weapon.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        if (!target) return;  // if target is null, return
        weapon.LookAt(target.transform);
    }
}
