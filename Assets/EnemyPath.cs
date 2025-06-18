using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] Transform[] points;

    [SerializeField] private float moveSpeed;
    private int Index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Index <= points.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[Index].transform.position, moveSpeed * Time.deltaTime);

            if(transform.position == points[Index].transform.position)
            {
                Index += 1;
            }
        }
    }
}
