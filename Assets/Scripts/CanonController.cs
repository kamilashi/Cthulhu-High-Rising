using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class CanonController : MonoBehaviour
{

    SphereCollider SphereCollider;
    public List<Transform> enemies;
    [SerializeField] private GameObject closestEnemy;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private int damage;

    public Transform CanonBody;
    bool enemyInRange;
    public GameObject bestTarget;
    private int arrayLength = 1;

    bool isShooting = false;
    IEnumerator shooting;

    public Projectile projectilePrefab;
    public Transform firePoint;

    IObjectPool<Projectile> projectilePool;

    // Start is called before the first frame update
    void Start()
    {
       enemies = new List<Transform>();
       shooting = Shoot();
        SphereCollider = this.GetComponent<SphereCollider>();
        SphereCollider.radius = AttackRange;

        //projectilePool = new ObjectPool<Projectile>(
        //createFunc: () =>
        //{
        //    var proj = Instantiate(projectilePrefab);
        //    proj.Pool = projectilePool;
        //    return proj;
        //},
        //    actionOnGet: (proj) => proj.gameObject.SetActive(true),
        //     actionOnRelease: (proj) => proj.gameObject.SetActive(false),
        //    actionOnDestroy: (proj) => Destroy(proj.gameObject),
        //    collectionCheck: false,
        //    defaultCapacity: 20,
        //    maxSize: 100
        //);

    }

    // Update is called once per frame
    void Update()
    {

        if (enemies.Count > 0)
        {
            if (bestTarget == null)
            {
                StopCoroutine(shooting);
                isShooting = false;
                findClosestEnemy();
            }
            else if(bestTarget != null)
            {
                Vector3 enemyDirection = bestTarget.transform.position - transform.position;
                
                //Quaternion targetRotation = Quaternion.LookRotation(enemyDirection);
                //CanonBody.transform.up = Quaternion.RotateTowards(CanonBody.transform.up, targetRotation, Time.deltaTime);


                //Vector3 enemyDirection = bestTarget.transform.position - transform.position;
                CanonBody.transform.up = enemyDirection;

                if (!isShooting)
                {
                    isShooting = true;
                    StartCoroutine(shooting);
                }
            }    

        }
        else
        {
            bestTarget = null;
            StopCoroutine(shooting);
            isShooting = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        enemyController enemy = other.GetComponent<enemyController>();
        if (enemy != null) 
        {
            Debug.Log("enemy in range");
            //addEnemy(other.transform);
            enemies.Add(other.transform);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemyController enemy = other.GetComponent<enemyController>();
        if (enemy != null)
        {
            Debug.Log("enemy out of range");
            
          
            enemies.Remove(other.transform);
        }
    }



    void findClosestEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                float closestDistance = Mathf.Infinity;
                float distanceToEnemy = (transform.position - enemy.position).magnitude;
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    bestTarget = enemy.gameObject;
                }
            }
           
        }
    }


    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/AttackSpeed);

            //var proj = projectilePool.Get();
            //proj.transform.position = firePoint.position;
            //proj.transform.rotation = firePoint.rotation;
            //proj.Launch(CanonBody.up, 20f);

            EnemyHealth enemyhealth = bestTarget.GetComponent<EnemyHealth>();
            enemyhealth.getHit(damage);
            if (enemyhealth.currentHealth == 0)
            {
                enemies.Remove(bestTarget.transform);
            }
            
        }
    }

}
