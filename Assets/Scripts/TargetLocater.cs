using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocater : MonoBehaviour
{
    Transform target;
    Transform weapon;
    void Awake()
    {
        target = GameObject.Find("Enemy").transform;
        weapon = transform.Find("BallistaTopMesh");
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        weapon.LookAt(target.transform);
    }
}
