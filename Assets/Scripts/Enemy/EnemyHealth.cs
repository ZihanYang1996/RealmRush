using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Enemy))]  // This will add the Enemy component if it doesn't already exist
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxhealth = 10;
    public int currentHealth { get; private set;}  // private set means that the value of currentHealth can only be set within this script
    
    Enemy enemy;
    IObjectPool<GameObject> enemyPool;

    void Awake()
    {
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
        enemy = gameObject.GetComponent<Enemy>();
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
            enemy.RewardGold();
        }
    }
}
