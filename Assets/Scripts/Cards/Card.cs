using System;
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
    public bool shouldBeDiscarded = false;

    public virtual void Play() { if (shouldBeDiscarded) { return; } }
    public virtual void RequestTargetSelection(Selectables selectionMode) { }
    public virtual ModifierTarget GetModifier() { return ModifierTarget.None; } // feels hacky, so far is only used in the deckViewer in a WIP function, so might have to be cleaned up

    public void Reset()
    {
        shouldBeDiscarded = false;

    }

    public void DisposeFromHand()
    {
        gameManager.deckSystem.DisCardToGraveyard(this);
        shouldBeDiscarded = true;
    }
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
        base.Play();
        gameManager.TryPlaceBlock(blockSO);

        DisposeFromHand();
    }

}
public class EquipmentCard : Card
{
    public EquipmentSO equipmentSO;

    public EquipmentCard()
    {
        type = CardType.Equipment;
        EventManager.onBlockSelectedEvent.AddListener(OnTargetSelected);
    }

    public override void Play()
    {
        base.Play();
        gameManager.RequestSelectionMode(Selectables.Block);
    }

    public void OnTargetSelected(Block block)
    {
        base.Play();
        gameManager.TryPlaceEquipment(block, equipmentSO);

        DisposeFromHand();
    }
}


[Serializable]
public class ModifierData
{

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
        base.Play();

        if(target == ModifierTarget.Block)
        {
            gameManager.RequestSelectionMode(Selectables.Block);
        }
        else
        {
            //gameManager.RequestSelectionMode(SelectionMode.Equipment);
        }

        DisposeFromHand(); // TEMP
    }

    public void OnTargetSelected(T target)
    {
        base.Play();

        //modify T

        DisposeFromHand();
    }
    public override ModifierTarget GetModifier() { return target; }
}