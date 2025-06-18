using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrap : MonoBehaviour
{

    Rigidbody rb;
    public Transform trapBody;
    [SerializeField] private float spinSpeed;
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        trapBody.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.getHit(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.getHit(damage);
        }
    }

}
