using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase
{
    Draw,
    Build,
    Combat
}

//MS: We need a BattleManager
public enum CombatResult
{
    Lost,
    Won
}

public static class EventManager
{
    public static UnityEvent<CardObject> onCardSelectedEvent = new UnityEvent<CardObject>();
    public static UnityEvent<Block> onBlockSelectedEvent = new UnityEvent<Block>();
    public static UnityEvent<Equipment> onEquipmentSelectedEvent = new UnityEvent<Equipment>();

    public static UnityEvent<Selectables, GameObject> onObjectSelectedEvent = new UnityEvent<Selectables, GameObject>(); // not used really

    public static UnityEvent<GamePhase> onGamePhaseChangedEvent = new UnityEvent<GamePhase>();

    public static UnityEvent onAllEnemiesDefeatedEvent = new();
    public static UnityEvent onEnemiesReachedTopEvent = new();

    public static UnityEvent onProceedEvent = new();
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
    public CombatResult? combatResult;
    public Selectables selectionMode;

    int currentWaveNumber = 0;

    bool continueNextPhase = false;

    public int getCurrentWave() => currentWaveNumber;
    public int getBlocksCount() => blockTower.blocks.Count;

    //public static GameManager Instance { get; private set; }

    void OnEnable()
    {
        EventManager.onProceedEvent.AddListener(OnProceedToNextPhase);
        EventManager.onAllEnemiesDefeatedEvent.AddListener(OnCombatVictory);
        EventManager.onEnemiesReachedTopEvent.AddListener(OnCombatLost);
    }

    void OnDisable()
    {
        EventManager.onAllEnemiesDefeatedEvent.RemoveListener(OnCombatVictory);
        EventManager.onEnemiesReachedTopEvent.AddListener(OnCombatLost);
    }

    void Awake()
    {
/*
        if (Instance != null && Instance != this)
        {
            Debug.LogAssertion("There should be only one instance of the GameManager!");
            Destroy(gameObject); 
            return;
        }*/

        deckSystem.gameManager = this;

        InitializeDeck();

        gamePhase = GamePhase.Draw;
        selectionMode = Selectables.None;
    }

    void Start()
    {
    }

    void Update()
    {
        GamePhase currentPhase = gamePhase;
        switch (gamePhase)
        {
            case GamePhase.Draw:
                if (deckSystem.hand.Count == 0)
                {
                    DrawHand();
                    gamePhase = GamePhase.Build;
                }
                break;
            case GamePhase.Build:
                if (continueNextPhase)
                {
                    DiscardHand();

                    currentWaveNumber++;

                    gamePhase = GamePhase.Combat;
                }
                break;
            case GamePhase.Combat:
                if(combatResult != null)
                {
                    if (continueNextPhase)
                    {
                        if(combatResult == CombatResult.Lost)
                        {
                            currentWaveNumber = 0;
                        }

                        gamePhase = GamePhase.Draw;
                        combatResult = null;
                    }
                }
                break;
        }

        if (currentPhase != gamePhase)
        {
            continueNextPhase = false;

            EventManager.onGamePhaseChangedEvent.Invoke(gamePhase);
        }

        if (deckSystem.hand.Count == 0)
        {
            ResetSelectionMode();
        }
    }

    void InitializeDeck()
    {
        deckSystem.Initialize(gameSettingsSO.defaultCardDeck);
    }

    [ContextMenu("DrawHand")]
    void DrawHand()
    {
        if(deckSystem.hand.Count != 0)
        {
            DiscardHand();
        }

        deckSystem.DrawHand();
        deckVisualizer.SpawnCards(deckSystem.hand);

        RequestSelectionMode(Selectables.Card);
    }

    [ContextMenu("DiscardHand")]
    void DiscardHand()
    {
        deckVisualizer.DespawnCards();
        deckSystem.DiscardHand();
    }

    public bool TryPlaceBlock(BlockSO blockSO)
    {
        // here can be a check for resources

        blockTower.CreateBlock(blockSO);

        return true;
    }

    public bool TryPlaceEquipment(Block block, EquipmentSO equipmentSO)
    {
        // here can be a check for resources

        if (block.HasFreeEquipmentSlots()) 
        {
            block.CreateEquipment(equipmentSO);
            return true;
        }

        return false;
    }

    public void RequestSelectionMode(Selectables mode)
    {
        selectionMode = mode;
    }

    public void ResetSelectionMode()
    {
        selectionMode = Selectables.None;
    }

    public void OnCardSelected(CardObject cardObject)
    {
        cardObject.card.OnSelected();

        EventManager.onCardSelectedEvent.Invoke(cardObject);
        EventManager.onObjectSelectedEvent.Invoke(Selectables.Card, cardObject.gameObject);
    }

    public void OnBlockSelected(Block block)
    {
        EventManager.onBlockSelectedEvent.Invoke(block);
        EventManager.onObjectSelectedEvent.Invoke(Selectables.Block, block.gameObject);

        RequestSelectionMode(Selectables.Card);
    }

    public void OnEquipmentSelected(Equipment equipment)
    {
        EventManager.onEquipmentSelectedEvent.Invoke(equipment);
        EventManager.onObjectSelectedEvent.Invoke(Selectables.Equipment, equipment.gameObject);

        RequestSelectionMode(Selectables.Card);
    }

    void OnCombatVictory()
    {
        Debug.Log("Combat ended � all enemies defeated!");
        combatResult = CombatResult.Won;
    }

    void OnCombatLost()
    {
        if(gamePhase == GamePhase.Combat)
        {
            Debug.Log("Combat ended � enemies reached the top!");
            combatResult = CombatResult.Lost;
        }
    }

    public void OnProceedToNextPhase()
    {
        continueNextPhase = true;
    }
}
