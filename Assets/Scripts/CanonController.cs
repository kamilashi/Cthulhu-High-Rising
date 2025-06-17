using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {

       enemies = new List<Transform>();
       shooting = Shoot();
        SphereCollider = this.GetComponent<SphereCollider>();
        SphereCollider.radius = AttackRange;
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
            
            float closestDistance = Mathf.Infinity;
            float distanceToEnemy = (transform.position - enemy.position).magnitude;
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                bestTarget = enemy.gameObject;
            }
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/AttackSpeed);

            enemyHealth enemyhealth = bestTarget.GetComponent<enemyHealth>();
            enemyhealth.getHit(damage);
            if (enemyhealth.currentHealth == 0)
            {
                enemies.Remove(bestTarget.transform);
            }
           // Destroy(bestTarget.gameObject);
        }
    }


}
