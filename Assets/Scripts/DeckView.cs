using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    [Header("Setup")]
    public GameManager gameManager;
    public GameObject cardObjectPrefab; // Later might be different prefabs
    public float distanceFromCamera;

    [Header("Auto Setup")]
    public Camera mainCamera;

    [Header("Debug View")]
    public List<CardObject> cardObjects;

    void Awake()
    {
        mainCamera = gameManager.mainCamera;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCards(List<Card> cards)
    {
        // assuming that cards are 1 m in width
        const float cardWidth = 1.0f;

        float projectedCardWidth = cardWidth / distanceFromCamera;
        //float widthRange = GetHorizontalRangeAtCameraDepth(distanceFromCamera, mainCamera.fieldOfView);
        float padding = 0.1f; //Screen.width / cards.Count - projectedCardWidth;
        padding = 1.5f * padding;

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject cardGO = Instantiate<GameObject>(cardObjectPrefab, mainCamera.transform);
            CardObject cardObject = cardGO.GetComponent<CardObject>();

            float multiplier = Mathf.Ceil(i / 2.0f);
            float sign = i % 2 == 0 ? -1 : 1;
            float horizontalOffset = (cardWidth + padding) * multiplier * sign;

            Vector3 position = mainCamera.transform.position;
            position.z += distanceFromCamera;
            position.x += horizontalOffset;

            float verticalOffset = GetDeltaYToScreenBottom(position, mainCamera.transform.position, mainCamera.fieldOfView);

            position.y += verticalOffset; // snap the pivot to the bottom of the screen

            cardObject.card = cards[i];
            cardObject.deckView = this;
            cardObject.transform.position = position;

            cardObject.nameText.text = cards[i].type.ToString();
            cardObject.descriptionText.text = GetDescriptionText(cards[i]);

            cardObjects.Add(cardObject);
        }
    }

    public void DespawnCard(CardObject cardObject)
    {
        int index = cardObjects.IndexOf(cardObject);

        // might need special handling since some cards need to stay around for a second action,
        // or destroying the card might have to be delegated to the deck logic

        DespawnCard(index);
        cardObjects.Remove(cardObject);
    }

    public void DespawnCards()
    {
        for (int i = 0; i < cardObjects.Count; i++)
        {
            DespawnCard(i);
        }

        cardObjects.Clear();
    }

    private void DespawnCard(int index)
    {

#if UNITY_EDITOR
        DestroyImmediate(cardObjects[index].gameObject);
#else
            Destroy(cardObjects[index].gameObject);
#endif
    }

    // this is temporary and needs to be replaced with something proper
    public string GetDescriptionText(Card card)
    {
        string text = "";

        switch(card.type)
        {
            case CardType.Block:
                {
                    BlockCard cardCard = card as BlockCard;
                    text += "Will create a " + cardCard.blockSO.name.ToString();
                    text += "\nHere should be stats";
                    break; 
                }
            case CardType.Equipment:
                {
                    text += "Choose a block to equip with ???. \n";
                    EquipmentCard cardCard = card as EquipmentCard;
                    text += "Here should be stats";
                    break; 
                }
            case CardType.Modifier:
                {
                    if(card.GetModifier() == ModifierTarget.Block)
                    {
                        ModifierCard<Block> cardCard = card as ModifierCard<Block>;
                        text += "Choose a block to modify with ??? \n";
                    }
                    else
                    {
                        ModifierCard<Block> cardCard = card as ModifierCard<Block>;
                        text += "Choose an equipment to modify with ??? \n";
                    }
                    break; 
                }
        }

        return text;
    }

    float GetDeltaYToScreenBottom(UnityEngine.Vector3 objectPos, UnityEngine.Vector3 currentCamPos, float vertFOV)
    {
        float distanceFromCamera = System.Math.Abs(objectPos.z - currentCamPos.z);
        float deltaObjY = distanceFromCamera * (float)System.Math.Tan(vertFOV * 0.5 * (System.Math.PI / 180.0));

        float deltaObjYtoCam0 = currentCamPos.y - objectPos.y;
        return deltaObjYtoCam0 - deltaObjY;
    }

    float GetHorizontalRangeAtCameraDepth(float depth, float vertFOV)
    {
        float verticalFOV = Camera.main.fieldOfView * Mathf.Deg2Rad;
        float aspect = Camera.main.aspect;

        float height = 2f * depth * Mathf.Tan(verticalFOV / 2f);
        float width = height * aspect;
        return width;
    }
}
