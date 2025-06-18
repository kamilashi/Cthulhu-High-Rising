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

    public int slots;

    public float height = 1.0f;
}
public class Block : MonoBehaviour
{
    public BlockData data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
