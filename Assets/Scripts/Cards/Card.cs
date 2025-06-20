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

[Serializable]
public struct CardData
{
    public string name;
    public string description;
}

public abstract class Card
{
    public CardData cardData;
    public GameManager gameManager;
    public CardType cardType;
    public bool shouldBeDiscarded = false;

    public Material material;

    public virtual void Play() { }
    public virtual void RequestTargetSelection(Selectables selectionMode) { }
    public virtual ModifierTarget GetModifier() { return ModifierTarget.None; } // feels hacky, so far is only used in the deckViewer in a WIP function, so might have to be cleaned up

    public void Reset()
    {
        shouldBeDiscarded = false;
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

    public string GetDescription()
    {
        return cardData.description;
    }
    public string GetName()
    {
        return cardData.name;
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
    }

    public override void Play()
    {
        if (IsDisabled())
        {
            return;
        }

        gameManager.RequestSelectionMode(Selectables.Block);

        EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
    }

    public void OnBlockTargetSelected(Block block)
    {
        if (IsDisabled())
        {
            return;
        }

        gameManager.TryPlaceEquipment(block, equipmentSO);

        EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
        DisposeFromHand();
    }
}

public class ModifierCard : Card
{
    public ModifierData modifierData;
    public object operand;
    public static int modifiedUnitsCount;
    public static int currentModifiedUnitsCount;
    public ModifierCard()
    {
        cardType = CardType.Modifier;
    }

    // todo: add removing listener on deselect and on disposing from hand

    public override void Play()
    {
        if (IsDisabled())
        {
            return;
        }

        if (modifierData.target == ModifierTarget.Block)
        {
            EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
            gameManager.RequestSelectionMode(Selectables.Block);
        }
        else
        {
            EventManager.onEquipmentSelectedEvent.AddListener(OnEquipmentTargetSelected);
            gameManager.RequestSelectionMode(Selectables.Equipment);
        }
    }
    public void OnBlockTargetSelected(Block block)
    {
        if (!IsTargetCorrect(ModifierTarget.Block, modifierData.target) || IsDisabled() )
        {
            return;
        }

        ModifyTarget(block, modifierData, operand);

        EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
        DisposeFromHand();
    }
    public void OnEquipmentTargetSelected(Equipment equipment)
    {
        if (!IsTargetCorrect(ModifierTarget.Equipment, modifierData.target) || IsDisabled())
        {
            return;
        }

        ModifyTarget(equipment, modifierData, operand);

        currentModifiedUnitsCount++;

        if (currentModifiedUnitsCount >= modifiedUnitsCount)
        {
            DisposeFromHand();
            EventManager.onEquipmentSelectedEvent.RemoveListener(OnEquipmentTargetSelected);

            currentModifiedUnitsCount = 0;
            modifiedUnitsCount = 0;
        }
    }

    private bool IsTargetCorrect(ModifierTarget current, ModifierTarget correct)
    {
        return current == correct;
    }

    public override ModifierTarget GetModifier() { return modifierData.target; }
}