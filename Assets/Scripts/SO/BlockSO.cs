using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block", order = 2)]
public class BlockSO : ScriptableObject
{
    public Mesh mesh;
    
    public Material material;

    public BlockData blockData;
}
