using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTower : MonoBehaviour
{
    [Header("Setup")]
    public Transform topBlock;
    public BlockSO startBlock; 

    [Header("Testing")]
    public BlockSO blockScriptableObject; // should come from the card

    [Header("Debug View")]
    public List<Block> blocks = new List<Block>();
    void Start()
    {
        CreateBlock(startBlock);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateBlock(BlockSO blockTemplate)
    {
        Vector3 position = this.transform.position;

        if (blocks.Count > 0) 
        {
            position = blocks[blocks.Count - 1].transform.position;
            position.y += blocks[blocks.Count - 1].data.height * blocks[blocks.Count - 1].transform.localScale.y;
        }

        GameObject blockGO = Instantiate<GameObject>(blockTemplate.blockPrefab, this.transform);

        // this needs to be moved into a block.Initialize function
        Block block = blockGO.GetComponent<Block>();

        block.transform.position = position;

        block.data = blockTemplate.blockData;

        block.availableSlotCount = blockTemplate.startSlotCount;

        block.Initialize();

        blocks.Add(block);

        // Move startBlock up with each new block
        Vector3 pos = topBlock.position;
        pos.y += blocks[blocks.Count - 1].data.height * blocks[blocks.Count - 1].transform.localScale.y;
        topBlock.transform.position = pos;
    }

    [ContextMenu("AddBlock")]
    void AddTestBlock()
    {
        CreateBlock(blockScriptableObject);
    }

    [ContextMenu("ClearBlocks")]
    void ClearBlocks()
    {
        foreach (Block block in blocks)
        {
#if UNITY_EDITOR
            DestroyImmediate(block.gameObject);
#else
            Destroy(block.gameObject);
#endif
        }

        blocks.Clear();
    }
}
