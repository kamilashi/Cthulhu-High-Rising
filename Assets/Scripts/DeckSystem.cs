using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void PlayCard(Card card)
    {

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
                ModifierTarget modifierTarget = deckData.modifierCardCounts[i].cardSO.modifierTarget;
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
        card.gameManager = gameManager;
        card.blockSO = cardSO.blockSO;

        return card;
    }

    private EquipmentCard CreateEquipmentCard(EquipmentCardSO cardSO)
    {
        EquipmentCard card = new EquipmentCard();
        card.gameManager = gameManager;
        //card.blockSO = cardSO.blockSO;

        return card;
    }

    private ModifierCard<Block> CreateBlockModifierCard(ModifierCardSO cardSO)
    {
        ModifierCard<Block> card = new ModifierCard<Block>();
        card.target = ModifierTarget.Block;

        return card;
    }
    private ModifierCard<Equipment> CreateEquipmentModifierCard(ModifierCardSO cardSO)
    {
        ModifierCard<Equipment> card = new ModifierCard<Equipment>();
        card.target = ModifierTarget.Equipment;

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
