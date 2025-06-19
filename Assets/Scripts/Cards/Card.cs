using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modifiers;

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
    public CardType cardType;
    public bool shouldBeDiscarded = false;
    public bool awaitingInput = false;

    public virtual void Play() { }
    public virtual void RequestTargetSelection(Selectables selectionMode) { }
    public virtual ModifierTarget GetModifier() { return ModifierTarget.None; } // feels hacky, so far is only used in the deckViewer in a WIP function, so might have to be cleaned up

    public void Reset()
    {
        shouldBeDiscarded = false;
        awaitingInput = false;
    }

    public bool IsDisabled()
    {
        return (shouldBeDiscarded);
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
        cardType = CardType.Block;
    }

    public override void Play()
    {
        IsDisabled();
        gameManager.TryPlaceBlock(blockSO);

        DisposeFromHand();
    }

}
public class EquipmentCard : Card
{
    public EquipmentSO equipmentSO;

    public EquipmentCard()
    {
        cardType = CardType.Equipment;
        EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
    }

    public override void Play()
    {
        if (IsDisabled())
        {
            return;
        }

        gameManager.RequestSelectionMode(Selectables.Block);

        awaitingInput = true;
    }

    public void OnBlockTargetSelected(Block block)
    {
        if (!awaitingInput || IsDisabled())
        {
            return;
        }

        gameManager.TryPlaceEquipment(block, equipmentSO);

        DisposeFromHand();
    }
}

public class ModifierCard : Card
{
    public ModifierData modifierData;
    public object operand;
    public ModifierCard()
    {
        cardType = CardType.Modifier;

        EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
        EventManager.onEquipmentSelectedEvent.AddListener(OnEquipmentTargetSelected);
    }
    public override void Play()
    {
        if (IsDisabled())
        {
            return;
        }

        if (modifierData.target == ModifierTarget.Block)
        {
            gameManager.RequestSelectionMode(Selectables.Block);
        }
        else
        {
            gameManager.RequestSelectionMode(Selectables.Equipment);
        }

        awaitingInput = true;
    }
    public void OnBlockTargetSelected(Block block)
    {
        if (!awaitingInput || !IsTargetCorrect(ModifierTarget.Block, modifierData.target) || IsDisabled() )
        {
            return;
        }

        ModifyTarget(block, modifierData, operand);

        DisposeFromHand();
    }
    public void OnEquipmentTargetSelected(Equipment equipment)
    {
        if (!awaitingInput || !IsTargetCorrect(ModifierTarget.Equipment, modifierData.target) || IsDisabled())
        {
            return;
        }

        ModifyTarget(equipment, modifierData, operand);

        DisposeFromHand();
    }

    private bool IsTargetCorrect(ModifierTarget current, ModifierTarget correct)
    {
        return current == correct;
    }

    public override ModifierTarget GetModifier() { return modifierData.target; }
}