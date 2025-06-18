using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrap : MonoBehaviour
{

    Rigidbody rb;
    public Transform trapBody;
    [SerializeField] private float spinSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int damage;
    //public List<GameObject> enemies;
    //bool isActive = false;
    //IEnumerator attackCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        //enemies = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        //attackCoroutine = hitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        trapBody.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        //trapBody.transform.localRotation =  Quaternion.Euler(spinSpeed * Time.deltaTime, 0, 0);

        //if (enemies.Count > 0)
        //{
        //    isActive = true;
        //    StartCoroutine(attackCoroutine);
        //}
        //else
        //{
        //    isActive = false;
        //    StopCoroutine(attackCoroutine);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            //enemies.Add(other.gameObject);
            enemy.getHit(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.getHit(damage);
            //enemies.Remove(other.gameObject);
        }
    }

    //IEnumerator hitEnemy()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1 / attackSpeed);

                
    //        Debug.Log("hit");
    //        foreach (var enemy in enemies)
    //        {
    //           EnemyHealth enemyhp = enemy.GetComponent<EnemyHealth>();
    //            enemyhp.getHit(damage);
    //            if (enemyhp.currentHealth == 0)
    //            {
    //                enemies.Remove(enemy.gameObject);
    //            }

    //        }
            

    //    }
    //}

}
