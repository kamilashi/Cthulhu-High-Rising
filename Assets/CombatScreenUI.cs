using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatScreenUI : MonoBehaviour
{
    [Header("Setup")]
    public GameManager gameManager;

    [Header("Setup in Prefab")]
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    public Button restartButton;
    public Button continueButton;

    public Canvas combatScreenCanvas;
    public TextMeshProUGUI defeatWaveText;
    public TextMeshProUGUI heightReachedText;

    private void Awake()
    {
        HideCombatEndScreen();
        ResetScreens();
        //EventManager.onGamePhaseChangedEvent.AddListener(OnGamePhaseChanged);
        EventManager.onEnemiesReachedTopEvent.AddListener(ShowDefeatScreen);
        EventManager.onAllEnemiesDefeatedEvent.AddListener(ShowVictoryScreen);

        restartButton.onClick.AddListener(OnRestartGamePressed);
        continueButton.onClick.AddListener(OnContinueGamePressed);
    }

    void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        ShowCombatEndScreen();
    }

    void ShowDefeatScreen()
    {
        defeatWaveText.SetText("Waves Cleared: " + (gameManager.getCurrentWave() - 1));
        heightReachedText.SetText("Blocks placed: " + (gameManager.getBlocksCount()));
        defeatScreen.SetActive(true);
        ShowCombatEndScreen();
    }

/*
    void OnGamePhaseChanged(GamePhase gamePhase)
    {
        if(gamePhase == GamePhase.Draw)
        {
            HideCombatEndScreen();
            ResetScreens();
        }
    }*/

    void ResetScreens()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
    }
    void HideCombatEndScreen()
    {
        combatScreenCanvas.enabled = false;
    }
    void ShowCombatEndScreen()
    {
        combatScreenCanvas.enabled = true;
    }

    void OnContinueGamePressed()
    {
        HideCombatEndScreen();
        ResetScreens();
        EventManager.onProceedEvent?.Invoke();
    }
    void OnRestartGamePressed()
    {
        HideCombatEndScreen();
        ResetScreens();
        EventManager.onRestartEvent?.Invoke();
    }
}
