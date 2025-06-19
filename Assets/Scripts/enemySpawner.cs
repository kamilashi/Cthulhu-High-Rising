using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyHealth enemyPrefab;
        public int spawnCount;
        [HideInInspector] public IObjectPool<EnemyHealth> pool;
    }

    [Header("Enemy Types")]
    [SerializeField] List<EnemySpawnData> enemyTypes = new List<EnemySpawnData>();

    [Header("Settings")]
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] float spawnInterval = 1f;


    int activeEnemies = 0;
    bool isActive = false;
    Coroutine spawnRoutine;

    void OnEnable()
    {
        EventManager.onGamePhaseChangedEvent.AddListener(OnGamePhaseChanged);
    }

    void OnDisable()
    {
        EventManager.onGamePhaseChangedEvent.RemoveListener(OnGamePhaseChanged);
    }

    void Start()
    {
        foreach (var enemyData in enemyTypes)
        {
            var localData = enemyData; 
            enemyData.pool = new ObjectPool<EnemyHealth>(
                createFunc: () =>
                {
                    var enemy = Instantiate(localData.enemyPrefab);
                    enemy.Pool = localData.pool;
                    return enemy;
                },
                actionOnGet: enemy => enemy.gameObject.SetActive(true),
                actionOnRelease: enemy => enemy.gameObject.SetActive(false),
                actionOnDestroy: enemy => Destroy(enemy.gameObject),
                collectionCheck: false,
                defaultCapacity: 20,
                maxSize: 100
            );
        }
    }

    void OnGamePhaseChanged(GamePhase phase)
    {
        bool shouldBeActive = (phase == GamePhase.Combat);

        if (isActive == shouldBeActive)
        {
            return;
        }

        isActive = shouldBeActive;

        if (isActive)
        {
            spawnRoutine = StartCoroutine(SpawnEnemies());
        }
        else if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }

    IEnumerator SpawnEnemies()
    {
        foreach (var enemyData in enemyTypes)
        {
            for (int i = 0; i < enemyData.spawnCount; i++)
            {
                yield return new WaitForSeconds(spawnInterval);

                var enemy = enemyData.pool.Get();
                SpawnEnemy(enemy);
                Debug.Log($"Spawned {enemyData.enemyPrefab.name}");
            }
        }
    }

    void SpawnEnemy(EnemyHealth enemy)
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        );

        enemy.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        RegisterEnemy(enemy);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy(enemyTypes[0].pool.Get());
        }
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        activeEnemies++;
        enemy.OnDeath += HandleEnemyDeath;
    }

    void HandleEnemyDeath(EnemyHealth enemy)
    {
        activeEnemies--;
        enemy.OnDeath -= HandleEnemyDeath;

        if (activeEnemies <= 0)
        {
            Debug.Log("All enemies defeated!");
            EventManager.onAllEnemiesDefeatedEvent.Invoke();
        }
    }

}