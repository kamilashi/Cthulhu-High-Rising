using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public interface IHoverable
{
    public void OnStartHover();
    public void OnStopHover();
}

public enum Selectables
{
    None,
    Card,
    Block,
    Equipment
}

public class WorldObjectSelectSystem : MonoBehaviour
{
    [Header("Setup")]
    public GameManager gameManager;
    public LayerMask cardLayer;
    public LayerMask blockLayer;
    public LayerMask equipmentLayer;

    public bool selectAllBlockEquipment = true;
    //public float maxRayCastDistance = 100.0f;

    [Header("Debug View")]
    public Selectables hoverMode;
    public LayerMask selectableLayerMask;
    public GameObject hoveredObject;

    public int cardLayerIdx;
    public int blockLayerIdx;
    public int equipmentLayerIdx;

    private void Awake()
    {
        cardLayerIdx = GetSingleLayerIndex(cardLayer);
        blockLayerIdx = GetSingleLayerIndex(blockLayer);
        equipmentLayerIdx = GetSingleLayerIndex(equipmentLayer);

        int combinedMask =
        (1 << cardLayerIdx) |
        (1 << blockLayerIdx) |
        (1 << equipmentLayerIdx);
        selectableLayerMask = combinedMask;
    }

    void Update()
    {
        ProcessHovering();
        ProcessSelection();
    }

    // might turn into templated fucntions as well
    private void OnCardSelect(GameObject selected)
    {
        CardObject cardObject = selected.GetComponent<CardObject>();
        gameManager.OnCardSelected(cardObject);
    }
    private void OnBlockSelect(GameObject selected)
    {
        Block blockObject = selected.GetComponent<Block>();
        gameManager.OnBlockSelected(blockObject);
    }
    private void OnEquipmentSelect(GameObject selected)
    {
        Equipment equipmentObject = selected.GetComponent<Equipment>();
        gameManager.OnEquipmentSelected(equipmentObject);
    }
    private void OnEquipmentBlockSelect(GameObject selected)
    {
        Block ownerBlockObject = selected.GetComponent<Equipment>().blockOwner;
        for (int i = 0; i < ownerBlockObject.equipmentList.Count; i++)
        {
            gameManager.OnEquipmentSelected(ownerBlockObject.equipmentList[i]);
        }
    }

    private void OnObjectHover<T>(GameObject hovered) where T : IHoverable
    {
        T hoveredObject = hovered.GetComponent<T>();
        hoveredObject.OnStartHover();
    }
    private void OnObjectStopHover<T>(GameObject hovered) where T : IHoverable
    {
        T hoveredObject = hovered.GetComponent<T>();
        hoveredObject.OnStopHover();
    }

    private void OnEquipmentBlockHover(GameObject hovered)
    {
        Block ownerBlockObject = hovered.GetComponent<Equipment>().blockOwner;
        for (int i = 0; i < ownerBlockObject.equipmentList.Count; i++) 
        {
            ownerBlockObject.equipmentList[i].OnStartHover();
        }
    }
    private void OnEquipmentBlockStopHover(GameObject hovered)
    {
        Block ownerBlockObject = hovered.GetComponent<Equipment>().blockOwner;
        for (int i = 0; i < ownerBlockObject.equipmentList.Count; i++)
        {
            ownerBlockObject.equipmentList[i].OnStopHover();
        }
    }

    private bool ProcessSelection() // needs to be called after processHovering
    {
        if (gameManager.selectionMode == Selectables.None)
        {
            return false;
        }
        
        if (!Input.GetMouseButtonDown(0))
        {
            return false;
        }

        bool receivedSelection = false;
        
        switch (hoverMode)
        {
            case Selectables.None:
                break;
            case Selectables.Card: // cards can be selected even when the selection mode != cards
                {
                    receivedSelection = true;
                    OnCardSelect(hoveredObject);
                    break;
                }
            case Selectables.Block:
                {
                    if(gameManager.selectionMode == Selectables.Block)
                    {
                        receivedSelection = true;
                        OnBlockSelect(hoveredObject);
                    }
                    break;
                }
            case Selectables.Equipment:
                {
                    if (gameManager.selectionMode == Selectables.Equipment)
                    {
                        receivedSelection = true;

                        if(selectAllBlockEquipment)
                        {
                            OnEquipmentBlockSelect(hoveredObject);
                        }
                        else
                        {
                            OnEquipmentSelect(hoveredObject);
                        }
                    }
                    break;
                }
        }

        return receivedSelection;
    }

    private void ProcessHovering()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, selectableLayerMask))
        {
            if(hoveredObject != null && hit.transform.gameObject != hoveredObject)
            {
                ProcessStopHover(hoverMode);
            }

            int layerIndex = hit.transform.gameObject.layer;
            if (layerIndex == cardLayerIdx)
            {
                hoverMode = Selectables.Card;
                OnObjectHover<CardObject>(hit.transform.gameObject);
            }
            else if (layerIndex == blockLayerIdx)
            {
                hoverMode = Selectables.Block;
                OnObjectHover<Block>(hit.transform.gameObject);
            }
            else if (layerIndex == equipmentLayerIdx)
            {
                hoverMode = Selectables.Equipment;

                if(selectAllBlockEquipment)
                {
                    OnEquipmentBlockHover(hit.transform.gameObject);
                }
                else
                {
                    OnObjectHover<Equipment>(hit.transform.gameObject);
                }
            }

            hoveredObject = hit.transform.gameObject;
        }
        else
        {
            if(hoveredObject!=null)
            {
                ProcessStopHover(hoverMode);
            }

            hoverMode = Selectables.None;
            hoveredObject = null;
        } 
    }

    private void ProcessStopHover(Selectables mode)
    {
        switch (mode)
        {
            case Selectables.Card:
                {
                    OnObjectStopHover<CardObject>(hoveredObject);
                    break;
                }
            case Selectables.Block:
                {
                    OnObjectStopHover<Block>(hoveredObject);
                    break;
                }
            case Selectables.Equipment:
                {
                    if (selectAllBlockEquipment)
                    {
                        OnEquipmentBlockStopHover(hoveredObject);
                    }
                    else
                    {
                        OnObjectStopHover<Equipment>(hoveredObject);
                    }
                    break;
                }
            case Selectables.None:
                break;
        }
    }

    int GetSingleLayerIndex(LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    }
}
