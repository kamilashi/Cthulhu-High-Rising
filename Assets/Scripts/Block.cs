using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Ice,
    Spikes,
    Eco
}

[Serializable]
public class BlockData
{
    public BlockType blockType;

    public float height = 1.0f;
}
public class Block : MonoBehaviour
{
    [Header("Setup in Prefab")]
    public List<Transform> slots = new List<Transform>();

    [Header("Auto Setup")]
    public BlockData data;

    [Header("Debug View")]
    public List<Equipment> equipmentList = new List<Equipment>(); // may change how equipment is stored

    void Awake()
    {
        Debug.Assert(slots.Count != 0, "Please, assign slots in the prefab of " + this.name);
    }

    void Update()
    {
        
    }

    public void CreateEquipment(EquipmentSO equipmentSO)
    {
        int freeSlots = slots.Count - equipmentList.Count;
        int takenSlots = slots.Count - freeSlots;

        for (int i = takenSlots; i < slots.Count; i++)
        {
            GameObject EquipmentGO = Instantiate<GameObject>(equipmentSO.prefab, slots[i]);

            Equipment equipment = EquipmentGO.GetComponent<Equipment>();

            equipment.equipmentData = equipmentSO.equipmentData;    
            
            equipment.blockOwner = this;    

            equipmentList.Add(equipment);
        }
    }

    public bool HasFreeEquipmentSlots()
    {
        return equipmentList.Count < slots.Count; 
    }
}
