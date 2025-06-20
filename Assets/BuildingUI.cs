using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public WorldObjectSelectSystem WorldObjectSelectSystem;
    public GameManager GameManager;

    public Canvas MainCanvas;

    public int overallCost;
    public TMP_Text overallCostBody;
    public int OneTimeCost;
    public TMP_Text OneTimeCostBody;
    public TMP_Text cardDescriptionBody;
    public int TotalOverallCost;
    public TMP_Text TotalOverallCostBody;

    public Button startWave;
    private CardObject hoveredObject;

    // Start is called before the first frame update
    void OnEnable()
    {
        startWave.onClick.AddListener(StartWave);

    }

    private void Update()
    {
        MainCanvas.enabled = GameManager.gamePhase == GamePhase.Build;

        if(WorldObjectSelectSystem.hoveredObject != null )
        {

            hoveredObject = WorldObjectSelectSystem.hoveredObject.GetComponent<CardObject>();
            if (hoveredObject!= null && cardDescriptionBody.text != hoveredObject.descriptionText.text )
            {

                UpdateCardDescription();
            }
            
        }
        else
        {
            hoveredObject = null;
            cardDescriptionBody.text = null;
            return;
        }  
        

    }

    // Update is called once per frame
    void UpdateCardDescription()
    {
        cardDescriptionBody.text = hoveredObject.descriptionText.text;
        if(hoveredObject == null)
        {
            cardDescriptionBody.text = null;
        }
    }

    void UpdateOverallCost()
    {
        overallCostBody.text = overallCost.ToString();

    }

    void UpdateOneTimeCost()
    {
        OneTimeCostBody.text = OneTimeCost.ToString();

    }

    public void StartWave()
    {
        if (GameManager.gamePhase == GamePhase.Build) 
        {
            Debug.Log("StartBattle");
            EventManager.onProceedEvent.Invoke();
        }
    }

}
