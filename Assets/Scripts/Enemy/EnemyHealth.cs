using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxhealth = 10;
    public int currentHealth { get; private set;}  // private set means that the value of currentHealth can only be set within this script
    
    IObjectPool<GameObject> enemyPool;

    void Awake()
    {
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
    }

    void OnEnable()
    {
        currentHealth = maxhealth;
    }

    void OnParticleCollision(GameObject other)
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            enemyPool?.Release(gameObject);
        }
    }
}
