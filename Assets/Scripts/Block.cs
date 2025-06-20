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

    public string name;

    public string description;
}

public class Block : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    public List<Transform> slots = new List<Transform>();

    [Header("Auto Setup")]
    public BlockData data;

    public int availableSlotCount;

    [Header("Debug View")]
    public List<Equipment> equipmentList = new List<Equipment>(); // how the equipment is stored may change later down the line

    public Color hoveredColor = Color.white;

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

        Debug.Assert(availableSlotCount == 0 || slots.Count != 0, "Please, assign slots in the prefab of " + this.name);
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

    public string GetName()
    {
        return data.name;
    }
    public string GetDescription()
    {
        return data.description;
    }

    public void OnStartHover()
    {
        if (isHovered) { return; }

        material.SetColor(hoveredColor);
        isHovered = true;
    }

    public void OnStopHover()
    {
        if (!isHovered) { return; }

        material.SetColor(Color.black);
        isHovered = false;
    }
}
