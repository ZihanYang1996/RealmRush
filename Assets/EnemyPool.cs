using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnTimer = 5f;
    [SerializeField] int defaultPoolSize = 10;
    [SerializeField] int maxPoolSize = 20;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    bool collectionChecks = true;

    IObjectPool<GameObject> objectPool;
    public IObjectPool<GameObject> ObjectPool
    {
        get
        {
            if (objectPool == null)
            {
                objectPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, defaultPoolSize, maxPoolSize);
            }
            return objectPool;
        }
    }

    GameObject CreatePooledItem()
    {
        // Will set the position and rotation later
        GameObject newEnemy = Instantiate(enemyPrefab);
        // Debug.Log("New enemy created");
        return newEnemy;
    }

    // It takes a GameObject as parameter
    // Set the taken object to active
    void OnTakeFromPool(GameObject item)
    {
        item.SetActive(true);
        // Debug.Log("Enemy taken from pool, " + ObjectPool.CountActive + " active enemies");
    }

    // It takes a GameObject as parameter
    // Set the returned object to inactive
    void OnReturnedToPool(GameObject item)
    {
        item.SetActive(false);
        // Debug.Log("Enemy returned to pool, " + ObjectPool.CountInactive + " inactive enemies");
    }

    // It takes a GameObject as parameter
    // Destroy the object
    void OnDestroyPoolObject(GameObject item)
    {
        Destroy(item);
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject newEnemy = ObjectPool.Get();

            // The position and rotation maybe not needed to be set, as it will be set in EnemyMover.cs
            newEnemy.transform.position = transform.position;  // set the position to the spawner's position
            newEnemy.transform.rotation = Quaternion.identity;  // set the rotation to identity, meaning no rotation
            
            newEnemy.transform.SetParent(transform);  // or newEnemy.transform.parent = transform;
            newEnemy.name = "Enemy";

            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
