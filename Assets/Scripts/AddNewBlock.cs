using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AddNewBlock : MonoBehaviour
{
    [SerializeField] private GameObject StoneBlock;
    [SerializeField] private GameObject Woodblock;
    [SerializeField] private GameObject IceBlock;
    public GameObject TowerObject;
    public Transform BuildPosiiton;
    Vector3 target;
    public GameObject SelectedBlock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1") || SelectedBlock == null)
        {
            SelectedBlock = StoneBlock;
        }

        if (Input.GetKeyDown("2"))
        {
            SelectedBlock = Woodblock;
        }

        if (Input.GetKeyDown("3"))
        {
            SelectedBlock = IceBlock;
        }

        if (Input.GetKeyDown("mouse 0"))
        {
           
                target = new Vector3(TowerObject.transform.position.x, TowerObject.transform.position.y + 1, TowerObject.transform.position.z);
            

            Instantiate(SelectedBlock, BuildPosiiton.position, Quaternion.identity, TowerObject.transform);
           
        }
        if (TowerObject.transform.childCount > 2)
        {
            if (TowerObject.transform.position != target)
            {
                var step = 3 * Time.deltaTime; // calculate distance to move
                TowerObject.transform.position = Vector3.MoveTowards(TowerObject.transform.position, target, step);

            }
        }
    }

}
