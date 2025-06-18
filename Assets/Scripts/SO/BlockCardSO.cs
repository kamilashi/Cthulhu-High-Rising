using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockCard", menuName = "ScriptableObjects/BlockCard", order = 3)]
public class BlockCardSO : CardSO
{
    public BlockSO blockSO;

    public BlockCardSO()
    {
        cardType = CardType.Block;
    }
}
