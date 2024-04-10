using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnTimer = 5f;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(transform);  // or newEnemy.transform.parent = transform;
            newEnemy.name = "Enemy";
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
