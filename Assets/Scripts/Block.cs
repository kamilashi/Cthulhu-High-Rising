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

    public string givenName;
}

public class Block : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    public List<Transform> slots = new List<Transform>();

    [Header("Auto Setup")]
    public BlockData data;

    [Header("Debug View")]
    public List<Equipment> equipmentList = new List<Equipment>(); // may change how equipment is stored

    // temporary
    // public Color regularColor;
    public Color hoveredColor = Color.blue;
    // public Material sharedMaterial;

    public MaterialValueAdjust material;

    public bool isHovered = false;

    void Awake()
    {
        Debug.Assert(slots.Count != 0, "Please, assign slots in the prefab of " + this.name);
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
