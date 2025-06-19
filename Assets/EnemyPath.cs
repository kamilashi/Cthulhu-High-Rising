using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<Transform> Nodes;
    [SerializeField] LayerMask FistNodeLayer;
    [SerializeField] Transform GroundPos;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    public int Index;

    // Start is called before the first frame update
    void Awake()
    {
            Physics.IgnoreLayerCollision(7, 7);
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Index <= Nodes.Count -1)
        {
            transform.position = Vector3.MoveTowards(transform.position, Nodes[Index].transform.position, moveSpeed * Time.deltaTime);

            Vector3 direction = Nodes[Index].position - transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            float PointDistance = (Nodes[Index].transform.position - transform.position).magnitude;

            if (PointDistance <= 0.2f)
            {
                if (Index == 0)
                {
                    var NextNode = Nodes[Index].GetComponent<FirstPathfindingNode>().NextNode;

                    if (NextNode != null)
                    {
                        Nodes.Add(NextNode.transform);
                        Index += 1;
                    }
                }

                   else if (Index >= 1)
                {
                    var NextNode = Nodes[Index].GetComponent<PathfindingNode>().NextNode;

                    if (NextNode != null)
                    {
                        Nodes.Add(NextNode.transform);
                        Index += 1;
                    }
                }
                
            }

            if(Index >= Nodes.Count)
            {
                Index = Nodes.Count - 1;
            }
        }
    }

    public void FindFirstPoint(Transform PointToAdd)
    {   
        Nodes = new List<Transform>();
        Nodes.Add(PointToAdd.transform);
        Index = 0;       
    }
}
