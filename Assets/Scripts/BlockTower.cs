using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTower : MonoBehaviour
{
    [Header("Setup")]
    public GameObject blockPrefab;

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
        MeshFilter meshFilter = blockGO.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = blockGO.GetComponent<MeshRenderer>();

        block.transform.position = position;

        block.data = blockTemplate.blockData;

        meshFilter.mesh = blockTemplate.mesh;
       
        meshRenderer.material = blockTemplate.material;



        blocks.Add(block);
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
