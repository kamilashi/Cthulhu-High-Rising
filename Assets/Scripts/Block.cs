using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum BlockType
{
    Default,
    Slow,
    DoT
}*/

[Serializable]
public struct BlockData
{
    //public BlockType blockType;

    public float height;

    public string givenName;
}

public class Block : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    public List<Transform> slots = new List<Transform>();

    [Header("Auto Setup")]
    public BlockData data;

    public int availableSlotCount;

    [Header("Debug View")]
    public List<Equipment> equipmentList = new List<Equipment>(); // may change how equipment is stored

    // public Color regularColor;
    public Color hoveredColor = Color.blue;
    // public Material sharedMaterial;

    public MaterialValueAdjust material;

    public bool isHovered = false;

    void Awake()
    {
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        // this is temporary, until a better on hovered solution is implemented
        // MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        // regularColor = meshRenderer.material.color;
        // sharedMaterial = meshRenderer.sharedMaterial;

        Debug.Assert(availableSlotCount > 0 && slots.Count != 0, "Please, assign slots in the prefab of " + this.name);
    }

    public void CreateEquipment(EquipmentSO equipmentSO)
    {
        int freeSlotsCount = slots.Count - equipmentList.Count;
        int freeSlotsStart = slots.Count - freeSlotsCount;
        int filledSlots = 0;

        for (int i = freeSlotsStart; i < slots.Count; i++)
        {
            GameObject EquipmentGO = Instantiate<GameObject>(equipmentSO.prefab, slots[i]);

            Equipment equipment = EquipmentGO.GetComponentInChildren<Equipment>();

            equipment.equipmentData = new EquipmentData( equipmentSO.baseData, equipmentSO.givenName);    
            
            equipment.blockOwner = this;

            equipment.Initialize();

            equipmentList.Add(equipment);

            filledSlots++;

            if (filledSlots >= availableSlotCount) 
            {
                break;
            }
        }

        availableSlotCount -= filledSlots;
    }

    public bool HasFreeEquipmentSlots()
    {
        return availableSlotCount > 0; 
    }

    public void OnStartHover()
    {
        if (isHovered) { return; }

        // sharedMaterial.color = hoveredColor;
        material.SetColor(hoveredColor);
        isHovered = true;
    }

    public void OnStopHover()
    {
        if (!isHovered) { return; }

        // sharedMaterial.color = regularColor;
        material.SetColor(Color.black);
        isHovered = false;
    }
}
