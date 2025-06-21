using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    [Header("Setup in Prefab")]
    public Canvas buildingScreenCanvas;

    public TMP_Text overallCostText;
    public TMP_Text oneTimeCostText;
    public TMP_Text cardDescriptionText;

    public Button startWaveButton;

    [Header("Debug View")]
    public int oneTimeCost;
    public int overallCost;
    public int totalOverallCost;

    void Awake()
    {
        startWaveButton.onClick.AddListener(OnStartWaveClicked);
        EventManager.onCardSelectedEvent.AddListener(UpdateCardDescription);
        EventManager.onGamePhaseChangedEvent.AddListener(OnGamePhaseChanged);
        HideBuildingScreen();
    }
    void OnGamePhaseChanged(GamePhase newPhase)
    {
        if(newPhase == GamePhase.Build)
        {
            ResetCardDescription();
            ShowBuildingScreen();
        }
    }

    void ShowBuildingScreen()
    {
        buildingScreenCanvas.enabled = true;
    }
    void HideBuildingScreen()
    {
        buildingScreenCanvas.enabled = false;
    }
    void ResetCardDescription()
    {
        cardDescriptionText.text = "";
    }

    void UpdateCardDescription(CardObject selectedCardOvject)
    {
        cardDescriptionText.text = selectedCardOvject.descriptionText.text;
    }

    void UpdateOverallCost()
    {
        overallCostText.text = overallCost.ToString();
    }

    void UpdateOneTimeCost()
    {
        oneTimeCostText.text = oneTimeCost.ToString();
    }

    public void OnStartWaveClicked()
    {
        //if (gameManager.gamePhase == GamePhase.Build) 
        {
            Debug.Log("StartBattle");
            EventManager.onProceedEvent.Invoke();
            HideBuildingScreen();
        }
    }

}
