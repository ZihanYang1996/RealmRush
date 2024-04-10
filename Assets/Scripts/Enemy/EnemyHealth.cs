using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxhealth = 10;
    public int currentHealth { get; private set;}  // private set means that the value of currentHealth can only be set within this script


    void Start()
    {
        currentHealth = maxhealth;
    }

    void OnParticleCollision(GameObject other)
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
