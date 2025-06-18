using UnityEngine;
using UnityEngine.Pool;


//This could also be inside of some battle manager

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile projectilePrefab;
    public Transform firePoint;

    IObjectPool<Projectile> projectilePool;

    void Start()
    {
        projectilePool = new ObjectPool<Projectile>(
            createFunc: () =>
            {
                var proj = Instantiate(projectilePrefab);
                proj.Pool = projectilePool;
                return proj;
            },
            actionOnGet: (proj) => proj.gameObject.SetActive(true),
            actionOnRelease: (proj) => proj.gameObject.SetActive(false),
            actionOnDestroy: (proj) => Destroy(proj.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    void Update()
    {
        // Remove, only to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var proj = projectilePool.Get();
            proj.transform.position = firePoint.position;
            proj.transform.rotation = firePoint.rotation;
            proj.Launch(firePoint.forward, 30f);
        }
    }
}