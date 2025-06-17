using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CanonController : MonoBehaviour
{

    SphereCollider SphereCollider;
    public Transform[] enemies2;
    public List<Transform> enemies;
    [SerializeField] private GameObject closestEnemy;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private int damage;
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
            findClosestEnemy();

            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(shooting);
            }
        }
        else 
        { 
            StopCoroutine(shooting );
            return; 
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
            Debug.Log("enemy in range");
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
            enemies.Remove(bestTarget.transform);
            Destroy(bestTarget.gameObject);
        }
    }


}
