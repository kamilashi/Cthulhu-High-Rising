using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatScreenUI : MonoBehaviour
{
    [Header("Setup")]
    public GameManager GameManager;

    [Header("Setup in Prefab")]
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;

    public Canvas CombatEndCanvas;
    public TextMeshProUGUI DefeatWaveText;
    public TextMeshProUGUI HeightReachedText;

    private void Awake()
    {
        HideCombatEndScreen();
        ResetScreens();
        EventManager.onGamePhaseChangedEvent.AddListener(OnGamePhaseChanged);
        EventManager.onEnemiesReachedTopEvent.AddListener(ShowDefeatScreen);
        EventManager.onAllEnemiesDefeatedEvent.AddListener(ShowVictoryScreen);
    }

    void ShowVictoryScreen()
    {
        VictoryScreen.SetActive(true);
        ShowCombatEndScreen();
    }

    void ShowDefeatScreen()
    {
        DefeatWaveText.SetText("Waves Cleared: " + (GameManager.getCurrentWave() - 1));
        HeightReachedText.SetText("Blocks placed: " + (GameManager.getBlocksCount()));
        DefeatScreen.SetActive(true);
        ShowCombatEndScreen();
    }

    void OnGamePhaseChanged(GamePhase gamePhase)
    {
        if(gamePhase == GamePhase.Draw)
        {
            HideCombatEndScreen();
            ResetScreens();
        }
    }

    void ResetScreens()
    {
        VictoryScreen.SetActive(false);
        DefeatScreen.SetActive(false);
    }
    void HideCombatEndScreen()
    {
        CombatEndCanvas.enabled = false;
    }
    void ShowCombatEndScreen()
    {
        CombatEndCanvas.enabled = true;
    }
}
