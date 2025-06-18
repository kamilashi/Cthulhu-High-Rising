using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectSelectSystem : MonoBehaviour
{
    [Header("Setup")]
    public GameManager gameManager;
    public LayerMask cardLayer;
    public LayerMask blockLayer;
    public LayerMask equipmentLayer;
    public float maxRayCastDistance = 100.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
            {
                OnCardSelect(hit.transform.gameObject);
            }
        }
    }

    private void OnCardSelect(GameObject selected)
    {
        CardObject cardObject = selected.GetComponent<CardObject>();
        gameManager.OnCardSelected(cardObject);
    }
    private void OnCardHover(GameObject hovered)
    {
    }

    private void OnBlockSelect(GameObject selected)
    {
    }
    private void OnEquipmentelect(GameObject selected)
    {
    }

    // todo: add hover events
}
