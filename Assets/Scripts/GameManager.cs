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
}
