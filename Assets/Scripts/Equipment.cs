using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Modifiers;


[Serializable]
public struct BaseEquipmentData
{
    public int baseAttackDamage;
    public float baseAttackRange;
    public float baseAttackSpeed;
}

[Serializable]
public struct EquipmentData
{
    public ModifiableData<ModifiableInt> damage;
    public ModifiableData<ModifiableFloat> attackRange;
    public ModifiableData<ModifiableFloat> attackSpeed;

    public string givenName;

    public EquipmentData(BaseEquipmentData baseData, string name)
    {
        damage = new ModifiableData<ModifiableInt>();
        damage.baseValueContainer.value = baseData.baseAttackDamage;

        attackRange = new ModifiableData<ModifiableFloat>();
        attackRange.baseValueContainer.value = baseData.baseAttackRange;

        attackSpeed = new ModifiableData<ModifiableFloat>();
        attackSpeed.baseValueContainer.value = baseData.baseAttackSpeed;

        givenName = name;
    }
}

public class Equipment : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    public MaterialValueAdjust material;

    [Header("Auto Setup")]
    public EquipmentData equipmentData;
    public Block blockOwner;
    public GameManager gameManager;

    // temporary
    [Header("Debug View")]
    public Color hoveredColor = Color.white;
    public List<Material> sharedMaterials = new List<Material>();

    public bool isHovered = false;
    public void Initialize()
    {

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
