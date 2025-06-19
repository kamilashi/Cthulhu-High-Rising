using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

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
        Destroy(this.gameObject);
    }
}
