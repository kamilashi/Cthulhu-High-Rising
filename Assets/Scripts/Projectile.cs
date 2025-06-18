using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public IObjectPool<Projectile> Pool { get; set; }

    Rigidbody rb;
    float lifetime = 5f; // maybe make public -> in/decrease lifetime if necessary 
    float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        rb.velocity = direction.normalized * force;
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyHealth enemy = other.GetComponent<enemyHealth>();
        if (enemy != null)
        {
            Debug.Log("enemy in range");
            //addEnemy(other.transform);
            enemy.getHit(1);
            ReturnToPool();
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
       // ReturnToPool();
    }

    void ReturnToPool()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Pool?.Release(this);
    }
}