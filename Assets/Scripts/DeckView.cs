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
    public float basePadding = 1.0f;

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
        //float cardWidth = cardObjectPrefab.transform.localScale.x;

        //float projectedCardWidth = cardWidth / distanceFromCamera;
        float padding = basePadding /** cardObjectPrefab.transform.localScale.x*/; 
        //padding = 1.5f * padding;

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject cardGO = Instantiate<GameObject>(cardObjectPrefab, mainCamera.transform);
            CardObject cardObject = cardGO.GetComponent<CardObject>();

            float multiplier = Mathf.Ceil(i / 2.0f);
            float sign = i % 2 == 0 ? -1 : 1;
            float horizontalOffset = (/*cardWidth +*/ padding) * multiplier * sign;

            Vector3 position = mainCamera.transform.position;

            position += mainCamera.transform.forward * distanceFromCamera;
            position += GetVectorToScreenBottom(distanceFromCamera, position, mainCamera.transform, mainCamera.fieldOfView); // snap the pivot to the bottom of the screen
            position += mainCamera.transform.right * horizontalOffset;

            cardObject.card = cards[i];
            cardObject.gameManager = gameManager;
            cardObject.deckView = this;
            cardObject.transform.position = position;

            cardObject.nameText.text = GetCardName(cards[i]);
            cardObject.descriptionText.text = GetCardDescription(cards[i]);

            cardObject.meshRenderer.material = cards[i].material;

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

    private void ProcessUninitializedText(ref string text)
    {
        if (text == "")
        {
            text = "Please set the text in the scriptable object.";
        }
    }

    private string GetCardDescription(Card card)
    {
        string desc = card.GetDescription();

        ProcessUninitializedText(ref desc);

        return card.GetDescription();
    }
    private string GetCardName(Card card)
    {
        string name = card.GetName();
        ProcessUninitializedText(ref name);
        return name;
    }

    private string GetDebugDescriptionText(Card card)
    {
        string text = "";

        switch(card.cardType)
        {
            case CardType.Block:
                {
                    BlockCard cardCard = card as BlockCard;
                    text += "Will create a " + cardCard.blockSO.name.ToString() + "\n";
                    text += "Here should be stats";
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
                    ModifierCard cardCard = card as ModifierCard;
                    if (card.GetModifier() == ModifierTarget.Block)
                    {
                        text += "Choose a block to modify:\n";
                    }
                    else
                    {
                        text += "Choose equipment to modify:\n\n";
                    }

                    text += cardCard.modifierData.modifiablePropertyType.ToString() + "\n\n";

                    text += cardCard.modifierData.operation + " , " + cardCard.operand.ToString() + "\n";

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
