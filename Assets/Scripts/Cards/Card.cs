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
    public bool wasSelected = false;

    public Material material;

    // hacky, listeners need to be cleaned up properly
    protected int modifyEquipmentListeners;
    protected int modifyBlockListeners;
    protected int equipListeners;

    public virtual void OnSelected() {}
    public virtual void OnDeselected() { }
    public virtual void CleanUpListeners() { }
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

        CleanUpListeners();

        SetWasSelected(false);
    }

    public string GetDescription()
    {
        return cardData.description;
    }
    public string GetName()
    {
        return cardData.name;
    }
    protected void SetWasSelected(bool selected)
    {
        this.wasSelected = selected;
    }
    public void OnOtherObjectSelected(CardObject objectSelected)
    {
        if(wasSelected && objectSelected.card != this)
        {
            OnDeselected();
            SetWasSelected(false);
        }
    }
    public void OnOtherObjectSelected(Equipment objectSelected)
    {
        if (wasSelected)
        { 
            OnDeselected();
        }

        SetWasSelected(false);
    }
    public void OnOtherObjectSelected(Block objectSelected)
    {
        if (wasSelected)
        {
            OnDeselected();
        }

        SetWasSelected(false);
    }
}

public class BlockCard : Card
{
    public BlockSO blockSO;

    public BlockCard()
    {
        cardType = CardType.Block;
    }

    public override void OnSelected()
    {
        SetWasSelected(true);

        if (IsDisabled() || !gameManager.TryPlaceBlock(blockSO))
        { 
            return; 
        }

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

    public override void OnSelected()
    {
        if (IsDisabled() || wasSelected)
        {
            return;
        }

        gameManager.RequestSelectionMode(Selectables.Block);

        EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
        equipListeners++;

        EventManager.onCardSelectedEvent.AddListener(OnOtherObjectSelected);
        EventManager.onEquipmentSelectedEvent.AddListener(OnOtherObjectSelected);


        SetWasSelected(true);
    }
    public override void OnDeselected()
    {
        EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
        Debug.Log("Removed Equipment Card Listener");
    }

    public void OnBlockTargetSelected(Block block)
    {
        if (IsDisabled())
        {
            return;
        }

        EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);

        if (!gameManager.TryPlaceEquipment(block, equipmentSO))
        { 
            return; 
        }

        DisposeFromHand();
    }

    public override void CleanUpListeners()
    {
        for (int i = 0; i < equipListeners; i++)
        {
            EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
        }

        equipListeners = 0;
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

    public override void OnSelected()
    {
        if (IsDisabled() || wasSelected)
        {
            return;
        }

        if (modifierData.target == ModifierTarget.Block)
        {
            EventManager.onBlockSelectedEvent.AddListener(OnBlockTargetSelected);
            modifyBlockListeners++;

            gameManager.RequestSelectionMode(Selectables.Block);

            EventManager.onCardSelectedEvent.AddListener(OnOtherObjectSelected);
            EventManager.onEquipmentSelectedEvent.AddListener(OnOtherObjectSelected);
        }
        else
        {
            EventManager.onEquipmentSelectedEvent.AddListener(OnEquipmentTargetSelected);
            modifyEquipmentListeners++;

            gameManager.RequestSelectionMode(Selectables.Equipment);

            EventManager.onCardSelectedEvent.AddListener(OnOtherObjectSelected);
            EventManager.onBlockSelectedEvent.AddListener(OnOtherObjectSelected);
        }

        SetWasSelected(true);
    }
    public override void OnDeselected()
    {
        if(modifierData.target == ModifierTarget.Block)
        {
            EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
            Debug.Log("Removed Block Modifier Listener");
        }
        else if(modifierData.target == ModifierTarget.Equipment)
        {
            EventManager.onEquipmentSelectedEvent.RemoveListener(OnEquipmentTargetSelected);
            Debug.Log("Removed Equipment Modifier Listener");
        }
    }

    public override void CleanUpListeners()
    {
        for (int i = 0; i < modifyBlockListeners; i++)
        {
            EventManager.onBlockSelectedEvent.RemoveListener(OnBlockTargetSelected);
        }

        for (int i = 0; i < modifyEquipmentListeners; i++)
        {
            EventManager.onEquipmentSelectedEvent.RemoveListener(OnEquipmentTargetSelected);
        }

        modifyBlockListeners = 0;
        modifyEquipmentListeners = 0;
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