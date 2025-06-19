using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modifiers;

public class DeckSystem : MonoBehaviour
{
    [Header("Setup")]
    public int handCount = 3; // will come from game settings

    [Header("Auto Setup")]
    public GameManager gameManager;

    [Header("Debug View")]
    public List<Card> cardDeck = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> graveyard = new List<Card>();

    void Start()
    {
        
    }
    void Awake()
    {
        
    }

    void Update()
    {
        
    }

    public void DisCardToGraveyard(Card card)
    {
        graveyard.Add(card);
        hand.Remove(card);
    }

    public void Initialize(DeckData deckData)
    {
        for (int i = 0; i < deckData.blockCardCounts.Count; i++)
        {
            for(int j = 0;  j < deckData.blockCardCounts[i].count; j++)
            {
                BlockCard card = CreateBlockCard(deckData.blockCardCounts[i].cardSO);
                cardDeck.Add(card);
            }
        }

        for (int i = 0; i < deckData.equipmentCardCounts.Count; i++)
        {
            for(int j = 0;  j < deckData.equipmentCardCounts[i].count; j++)
            {
                EquipmentCard card = CreateEquipmentCard(deckData.equipmentCardCounts[i].cardSO);
                cardDeck.Add(card);
            }
        }

        for (int i = 0; i < deckData.modifierCardCounts.Count; i++)
        {
            for(int j = 0;  j < deckData.modifierCardCounts[i].count; j++)
            {
                ModifierTarget modifierTarget = deckData.modifierCardCounts[i].cardSO.modifierData.target;
                if (modifierTarget == ModifierTarget.Block)
                {
                    cardDeck.Add(CreateBlockModifierCard(deckData.modifierCardCounts[i].cardSO));
                }
                else if(modifierTarget == ModifierTarget.Equipment)
                {
                    cardDeck.Add(CreateEquipmentModifierCard(deckData.modifierCardCounts[i].cardSO));
                }
            }
        }
    }

    private BlockCard CreateBlockCard(BlockCardSO cardSO)
    {
        BlockCard card = new BlockCard();
        card.material = cardSO.material;
        card.gameManager = gameManager;
        card.blockSO = cardSO.blockSO;

        return card;
    }

    private EquipmentCard CreateEquipmentCard(EquipmentCardSO cardSO)
    {
        EquipmentCard card = new EquipmentCard();
        card.gameManager = gameManager;
        card.material = cardSO.material;
        card.equipmentSO = cardSO.equipmentSO;

        return card;
    }

    // todo: combine CreateBlockModifierCard and CreateEquipmentModifierCard into CreateModifierCard!
    private ModifierCard CreateBlockModifierCard(ModifierCardSO cardSO)
    {
        ModifierCard card = new ModifierCard();
        card.gameManager = gameManager;
        card.modifierData = cardSO.modifierData;
        card.material = cardSO.material;

        object operandConverted = cardSO.operand;

        if (cardSO.modifierData.modifiablePropertyType.GetType() == typeof(ModifiableInt))
        {
            Debug.Assert(cardSO.operand - Mathf.Floor(cardSO.operand) != 0.0f, "please, define a fractionless operand for " + cardSO.name);

            operandConverted = Mathf.FloorToInt(cardSO.operand);
        }

        card.operand = cardSO.operand;

        return card;
    }
    private ModifierCard CreateEquipmentModifierCard(ModifierCardSO cardSO)
    {
        ModifierCard card = new ModifierCard();
        card.gameManager = gameManager;
        card.modifierData = cardSO.modifierData;
        card.material = cardSO.material;

        object operandConverted = cardSO.operand;

        if(cardSO.modifierData.modifiablePropertyType.GetType() == typeof(ModifiableInt))
        {
            Debug.Assert(cardSO.operand - Mathf.Floor(cardSO.operand) != 0.0f, "please, define a fractionless operand for " + cardSO.name);

            operandConverted = Mathf.FloorToInt(cardSO.operand);
        }

        card.operand = cardSO.operand;

        return card;
    }

    public void DrawHand()
    {
        int handSize = handCount;

        if (cardDeck.Count == 0)
        {
            RestockFromGraveyard();
        }

        for (int i = 0; i < handSize; i++) 
        {
            int randomIndex = UnityEngine.Random.Range(0, cardDeck.Count);
            Card card = cardDeck[randomIndex];
            hand.Add(card);
            cardDeck.RemoveAt(randomIndex);

            card.Reset();
        }
    }

    public void DiscardHand()
    {
        graveyard.AddRange(hand);
        hand.Clear();
    }
    public void RestockFromGraveyard()
    {
        cardDeck.AddRange(graveyard);
        graveyard.Clear();
    }
}
