using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHealth : MonoBehaviour
{
    public event Action<EnemyHealth> OnDeath;

    public int maxHealth;
    public int currentHealth;
    private Rigidbody rb;
    public IObjectPool<EnemyHealth> Pool { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void getHit(int Damage) 
    {
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            Invoke(nameof(Die), 0.5f);
        }
    }

    void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
        Pool?.Release(this);
    }

    void ReturnToPool()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Pool?.Release(this);
    }
}
