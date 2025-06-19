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

    bool isDead = false;

    void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void getHit(int Damage) 
    {
        currentHealth -= Damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isDead)
        {
            isDead = true;

            OnDeath?.Invoke(this);
            Pool?.Release(this);
        }
    }
}
