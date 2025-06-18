using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class enemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform ClimbstartPosition;
    private Rigidbody rb;
    Vector3 desiredPosition;
    Vector3 startPosition;

    bool reachedTower = false;
    [SerializeField] LayerMask towerLayer;

    Vector3 towerDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (ClimbstartPosition == null)
            ClimbstartPosition = GameObject.FindWithTag("Tower").transform;

        desiredPosition = new Vector3(ClimbstartPosition.position.x + Random.Range(-1,1), 0, ClimbstartPosition.position.z + Random.Range(-1, 1));
        towerDirection = (ClimbstartPosition.position - transform.position);
        transform.forward = towerDirection;
        Physics.IgnoreLayerCollision(6,6);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        reachedTower = Physics.Raycast(transform.position, towerDirection, 0.2f, towerLayer);

        if (!reachedTower)
        {
            rb.velocity = towerDirection * movementSpeed * Time.deltaTime;
        }
        else
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(0,5f,0) * movementSpeed * Time.deltaTime;
        }
      

    }
}
