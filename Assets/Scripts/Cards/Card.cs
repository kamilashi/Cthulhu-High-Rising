using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetOperator
{
    
}

public enum CardType
{
    Block,
    Equipment,
    Modifier
}

public enum ModifierTarget
{
    Block,
    Equipment,
    None
}

public abstract class Card
{
    public GameManager gameManager;
    public CardType type;

    public virtual void Play()
    {

    }
    public virtual ModifierTarget GetModifier() { return ModifierTarget.None; } // feels hacky, so far is only used in the deckViewer in a WIP function, so might have to be cleaned up
}

public class BlockCard : Card
{
    public BlockSO blockSO;

    public BlockCard()
    {
        type = CardType.Block;
    }

    public override void Play()
    {
        gameManager.blockTower.CreateBlock(blockSO);
    }

}
public class EquipmentCard : Card
{
    //public EquipmentSO equipmentSO;
    public EquipmentCard()
    {
        type = CardType.Equipment;
    }
    public override void Play()
    {
        //request block select
    }
    public void OnTargetSelected(Block block)
    {

    }
}

public class ModifierCard<T> : Card
{
    public ModifierTarget target;

    public ModifierCard()
    {
        type = CardType.Modifier;
    }
    public override void Play()
    {
        //either request block select or equipment select
    }

    public void OnTargetSelected(T target)
    {

    }
    public override ModifierTarget GetModifier() { return target; }
}