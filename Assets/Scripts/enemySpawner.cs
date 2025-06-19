using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class enemySpawner : MonoBehaviour
{
    [Header("Enemy option 1")]
    [SerializeField] private EnemyHealth enemyOption1;
    [SerializeField] private int spawnCount1;
    [Header("Enemy option 2")]
    [SerializeField] private EnemyHealth enemyOption2;
    [SerializeField] private int spawnCount2;
    [Header("Enemy option 3")]
    [SerializeField] private EnemyHealth enemyOption3;  
    [SerializeField] private int spawnCount3; 

    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnTime;

    IEnumerator spawnCoroutine;
    public List<GameObject> spawnList = new List<GameObject>();

    IObjectPool<EnemyHealth> enemyPool1;
    IObjectPool<EnemyHealth> enemyPool2;
    IObjectPool<EnemyHealth> enemyPool3;
    // Start is called before the first frame update
    void Start()
    {
        //AddToSpawnList(enemyOption1.gameObject, spawnCount1);
        //AddToSpawnList(enemyOption2.gameObject, spawnCount2);
        //AddToSpawnList(enemyOption3, spawnCount3);
        //spawnEnemies();
        
        spawnCoroutine = SpawnCoroutine(spawnCount1, spawnCount2, spawnCount3);
        StartCoroutine(spawnCoroutine);
       

        //Enemy pool for Type 1 Enemies
        enemyPool1 = new ObjectPool<EnemyHealth>(
         createFunc: () =>
         {
             var enemy1 = Instantiate(enemyOption1);
             
             enemy1.Pool = enemyPool1;
             
             return enemy1;
             
         },
         actionOnGet: (enemy1) => enemy1.gameObject.SetActive(true),
         actionOnRelease: (enemy1) => enemy1.gameObject.SetActive(false),
         actionOnDestroy: (enemy1) => Destroy(enemy1.gameObject),
         collectionCheck: false,
         defaultCapacity: 20,
         maxSize: 100
         );

        //Enemy pool for Type 2 Enemies
        enemyPool2 = new ObjectPool<EnemyHealth>(
         createFunc: () =>
         {
             var enemy2 = Instantiate(enemyOption1);

             enemy2.Pool = enemyPool2;

             return enemy2;

         },
         actionOnGet: (enemy2) => enemy2.gameObject.SetActive(true),
         actionOnRelease: (enemy2) => enemy2.gameObject.SetActive(false),
         actionOnDestroy: (enemy2) => Destroy(enemy2.gameObject),
         collectionCheck: false,
         defaultCapacity: 20,
         maxSize: 100
         );

        //Enemy pool for Type 3 Enemies
        enemyPool3 = new ObjectPool<EnemyHealth>(
         createFunc: () =>
         {
             var enemy3 = Instantiate(enemyOption1);

             enemy3.Pool = enemyPool1;

             return enemy3;

         },
         actionOnGet: (enemy3) => enemy3.gameObject.SetActive(true),
         actionOnRelease: (enemy3) => enemy3.gameObject.SetActive(false),
         actionOnDestroy: (enemy3) => Destroy(enemy3.gameObject),
         collectionCheck: false,
         defaultCapacity: 20,
         maxSize: 100
         );

    }

    private void Update()
    {
        // Remove, only to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnEnemyType1();
            
        }

    }

 

    public void spawnEnemyType1()
    {
        Debug.Log("spawnenemy");
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
        
            var enemy1 = enemyPool1.Get();
            enemy1.transform.position = transform.position;
            enemy1.transform.rotation = Quaternion.identity;

        
        
    }

    public void spawnEnemyType2()
    {
        Debug.Log("spawnenemy");
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
        
            var enemy2 = enemyPool2.Get();
            enemy2.transform.position = transform.position;
            enemy2.transform.rotation = Quaternion.identity;

        

    }

    public void spawnEnemyType3()
    {
        Debug.Log("spawnenemy");
        Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + Random.Range(-spawnRadius, spawnRadius));
        
            var enemy3 = enemyPool3.Get();
            enemy3.transform.position = transform.position;
            enemy3.transform.rotation = Quaternion.identity;

        

    }

    public IEnumerator SpawnCoroutine(int Spawn1, int Spawn2, int Spawn3)
    {
        while(true)
        {
            for (int i = 0; i < Spawn1; i++) { 
                yield return new WaitForSeconds(spawnTime);


                spawnEnemyType1();
                if(i >= Spawn1)
                {
                   yield return null;
                }
            }
            for (int i = 0; i < Spawn2; i++)
            {
                yield return new WaitForSeconds(spawnTime);


                spawnEnemyType2();
                if (i >= Spawn2)
                {
                    yield return null;
                }
            }
            for (int i = 0; i < Spawn3; i++)
            {
                yield return new WaitForSeconds(spawnTime);


                spawnEnemyType3();
                if (i >= Spawn2)
                {
                    yield return null;
                }
            }

        }
        
    }
}
