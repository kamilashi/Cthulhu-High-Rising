using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    Draw,
    Build,
    Combat
}

public class GameManager : MonoBehaviour
{

    [Header("Setup")]

    public GameSettingsSO gameSettingsSO;
    
    public BlockTower blockTower;
    public DeckSystem deckSystem;
    public DeckView deckVisualizer;
    public Camera mainCamera;
    public WorldObjectSelectSystem objectSelectSystem;


    [Header("Debug View")]
    public GamePhase gamePhase;

    void Awake()
    {
        deckSystem.gameManager = this;

        InitializeDeck();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeDeck()
    {
        deckSystem.Initialize(gameSettingsSO.defaultCardDeck);
    }

    [ContextMenu("DrawHand")]
    void DrawHand()
    {
        deckSystem.DrawHand();
        deckVisualizer.SpawnCards(deckSystem.hand);
    }

    [ContextMenu("DiscardHand")]
    void DiscardHand()
    {
        deckVisualizer.DespawnCards();
        deckSystem.DiscardHand();
    }

    public void OnCardSelected(CardObject cardObject)
    {
        cardObject.card.Play();

        //potential enter to a selection mode

        // only applies to block cards:
        deckSystem.DisCardToGraveyard(cardObject.card);
        deckVisualizer.DespawnCard(cardObject);
    }

/*
    public void OnBlockCardSelected(BlockCard card)
    {

    }
    public void OnEquipmentCardSelected(Equipment card)
    {

    }
    public void OnModifierCardSelected<MT>(ModifierCard<MT> card)
    {

    }*/
}
