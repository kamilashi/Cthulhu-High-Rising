using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class CanonController : MonoBehaviour
{

    [Header("Setup in Prefab")]
    public Equipment equipment;
    public LayerMask enemyLayer;
    public Projectile projectilePrefab;
    public Transform firePoint;
    public Transform CanonBody;

    [Header("Auto Setup")]
    public SphereCollider SphereCollider;

    [Header("Debug View")]
    public float attackRange;
    public float attackSpeed;
    public int damage;

    public GameObject closestEnemy;
    public GameObject bestTarget;

    List<Transform> enemies;

    bool isShooting = false;
    IEnumerator shooting;

    IObjectPool<Projectile> projectilePool;

    // Start is called before the first frame update
    void Start()
    {
       enemies = new List<Transform>();
       shooting = Shoot();
       SphereCollider = this.GetComponent<SphereCollider>();

       UpdateEquipmentProperties();

       EventManager.onGamePhaseChangedEvent.AddListener(OnPhaseChangedEvent);
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

    private void OnDestroy()
    {
        EventManager.onGamePhaseChangedEvent.RemoveListener(OnPhaseChangedEvent);
    }

    void OnPhaseChangedEvent(GamePhase newPhase)
    {
        if(newPhase == GamePhase.Combat)
        {
            UpdateEquipmentProperties();
        }
    }

    void UpdateEquipmentProperties()
    {
        attackRange = equipment.equipmentData.attackRange.GetAndStoreValue<float>();
        attackSpeed = equipment.equipmentData.attackSpeed.GetAndStoreValue<float>();
        damage = equipment.equipmentData.damage.GetAndStoreValue<int>();

        SphereCollider.radius = attackRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null) 
        {
            enemies.Add(other.transform);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        { 
            enemies.Remove(other.transform);
        }
    }

    void findClosestEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
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
            yield return new WaitForSeconds(1/attackSpeed);

            EnemyHealth enemyhealth = bestTarget.GetComponent<EnemyHealth>();
            enemyhealth.getHit(damage);
            if (enemyhealth.currentHealth <= 0)
            {
                enemies.Remove(bestTarget.transform);
                bestTarget = null;
            }
        }
    }

}
