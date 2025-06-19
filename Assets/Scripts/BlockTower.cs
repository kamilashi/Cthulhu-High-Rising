using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTower : MonoBehaviour
{
    [Header("Setup")]
    public GameObject blockPrefab;
    public Transform startBlock;

    [Header("Testing")]
    public BlockSO blockScriptableObject; // should come from the card

    [Header("Debug View")]
    public List<Block> blocks = new List<Block>();
    void Start()
    {
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

        GameObject blockGO = Instantiate<GameObject>(blockPrefab, this.transform);

        // this needs to be moved into a block.Initialize function
        Block block = blockGO.GetComponent<Block>();
        // MeshFilter meshFilter = blockGO.GetComponent<MeshFilter>();  // Note AV: this happens in block?
        // MeshRenderer meshRenderer = blockGO.GetComponent<MeshRenderer>(); // Note AV: this happens in block?

        block.transform.position = position;

        block.data = blockTemplate.blockData;

        //meshFilter.mesh = blockTemplate.mesh; // Note AV: this happens in block?
        // meshRenderer.material = blockTemplate.material;  // Note AV: this happens in block?

        block.Initialize();

        blocks.Add(block);

        // Move startBlock up with each new block
        Vector3 pos = startBlock.position;
        pos.y += block.data.height * block.transform.localScale.y;
        startBlock.position = pos;
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
