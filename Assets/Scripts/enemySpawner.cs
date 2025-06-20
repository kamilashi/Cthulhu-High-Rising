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
    [SerializeField] List<EnemySpawnData> enemyTypes = new();

    [Header("Settings")]
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] float spawnInterval = 1f;

    readonly List<EnemyHealth> enemies = new();

    float spawnMultiplier = 1.0f;
    
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
            IObjectPool<EnemyHealth> pool = null;

            pool = new ObjectPool<EnemyHealth>(
                createFunc: () =>
                {
                    var enemy = Instantiate(enemyData.enemyPrefab);
                    enemy.Pool = pool;
                    return enemy;
                },
                actionOnGet: enemy => enemy.gameObject.SetActive(true),
                actionOnRelease: enemy =>
                {
                    enemy.OnDeath -= HandleEnemyDeath;

                    var controller = enemy.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.OnReachedGoal -= HandleOnReachedTop;
                    }

                    enemy.gameObject.SetActive(false);
                },
                collectionCheck: false,
                defaultCapacity: 20,
                maxSize: 100
            );

            enemyData.pool = pool;
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
            int numberOfEnemiesToSpawn = (int) (spawnMultiplier * enemyData.spawnCount);

            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
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
        enemy.OnDeath += HandleEnemyDeath;

        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        if (enemyController)
        {
            enemyController.OnReachedGoal += HandleOnReachedTop;
        }

        enemies.Add(enemy);
    }

    void HandleEnemyDeath(EnemyHealth enemy)
    {
        enemies.Remove(enemy);
        enemy.OnDeath -= HandleEnemyDeath;

        if (enemies.Count == 0)
        {
            Debug.Log("All enemies defeated!");

            spawnMultiplier += 1.0f;

            EventManager.onAllEnemiesDefeatedEvent.Invoke();
        }

        
    }

    void HandleOnReachedTop(EnemyController enemy)
    {
        foreach(EnemyHealth e in enemies)
        {
            e.Release();
        }
        enemies.Clear();

        spawnMultiplier = 1.0f;

        EventManager.onEnemiesReachedTopEvent.Invoke();
    }
}