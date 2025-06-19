using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modifiers;


[Serializable]
public class EquipmentData
{
    public ModifiableData<ModifiableFloat> damage;
    public ModifiableData<ModifiableFloat> attackRange;
    public ModifiableData<ModifiableFloat> attackSpeed;

    public string givenName;
}

public class Equipment : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    public List<MeshRenderer> meshRenderers = new List<MeshRenderer>(); // temporary

    [Header("Auto Setup")]
    public EquipmentData equipmentData;
    public Block blockOwner;

    // temporary
    [Header("Debug View")]
    public List<Color> regularColors = new List<Color>();
    public Color hoveredColor = Color.blue;
    public List<Material> sharedMaterials = new List<Material>();

    public bool isHovered = false;
    public void Initialize()
    {
        // this is temporary, until a better on hovered solution is implemented
        for(int i = 0; i<meshRenderers.Count; i++)
        {
            regularColors.Add(meshRenderers[i].material.color);
            sharedMaterials.Add(meshRenderers[i].sharedMaterial);
        }

    }
    public void OnStartHover()
    {
        if (isHovered) { return; }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            sharedMaterials[i].color = hoveredColor;
        }

        isHovered = true;
    }

    public void OnStopHover()
    {
        if (!isHovered) { return; }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            sharedMaterials[i].color = regularColors[i];
        }

        isHovered = false;
    }
}
