using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPathfindingNode : MonoBehaviour
{
    public Transform NextNode;

    public void Awake()
    {
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            Debug.Log("enemy near node");
            enemy.FindFirstPoint(this.transform);
        }

    }
}
