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
