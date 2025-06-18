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

            position += mainCamera.transform.forward * distanceFromCamera;
            position += GetVectorToScreenBottom(distanceFromCamera, position, mainCamera.transform, mainCamera.fieldOfView); // snap the pivot to the bottom of the screen
            position += mainCamera.transform.right * horizontalOffset;

            cardObject.card = cards[i];
            cardObject.gameManager = gameManager;
            cardObject.deckView = this;
            cardObject.transform.position = position;

            cardObject.nameText.text = cards[i].type.ToString();
            cardObject.descriptionText.text = GetDescriptionText(cards[i]);

            cardObject.Initialize();

            cardObjects.Add(cardObject);
        }
    }

    public void DespawnCard(CardObject cardObject)
    {
        int index = cardObjects.IndexOf(cardObject);

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
                    EquipmentCard cardCard = card as EquipmentCard;
                    text += "Choose a block to equip with a " + cardCard.equipmentSO.name + "\n";
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

    UnityEngine.Vector3 GetVectorToScreenBottom(float distanceFromCamera, UnityEngine.Vector3 objectPos, UnityEngine.Transform cameraTransform, float vertFOV)
    {
        float deltaObjY = distanceFromCamera * (float)System.Math.Tan(vertFOV * 0.5 * (System.Math.PI / 180.0));

        UnityEngine.Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        targetPosition -= cameraTransform.up * deltaObjY;
        return targetPosition - objectPos;
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
