using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyOption1;
    [SerializeField] private GameObject enemyOption2;
    [SerializeField] private GameObject enemyOption3;

    [SerializeField] private float spawnRadius;
    [SerializeField] private int spawnCount1;
    [SerializeField] private int spawnCount2;
    [SerializeField] private int spawnCount3;
    // [SerializeField] private float spawnTime;

    public List<GameObject> spawnList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        AddToSpawnList(enemyOption1, spawnCount1);
        AddToSpawnList(enemyOption2, spawnCount2);
        AddToSpawnList(enemyOption3, spawnCount3);
        spawnEnemies();
    }

    public void AddToSpawnList(GameObject enemyToSpawn, int count)
    {

        for (int i = 0; i < count; i++)
        {
            spawnList.Add(enemyToSpawn);
        }
        
    }

    public void spawnEnemies()
    {
        foreach (GameObject enemy in spawnList)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
            Instantiate(enemy, spawnPosition, Quaternion.identity, this.transform);
            
        }

    }
}
