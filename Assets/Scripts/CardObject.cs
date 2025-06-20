using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour, IHoverable
{
    [Header("Setup in Prefab")]
    //- this will change when we have separate prefabs for different card types.
    // Once we do, each CardObject might have to handle card display info differently.
    public MeshRenderer meshRenderer;
    
    public TextMeshPro nameText;
    public TextMeshPro descriptionText;
    
    public GameObject hoverOutline; // this is temporary until a better solution


    [Header("Card Animation")]
    public float onHoverAnimationDistance = 3.0f;
    public float onHoverAnimationSpeed = 5.0f;

    [Header("AutoSetup")]
    public GameManager gameManager;
    public DeckView deckView;
    public Card card;

    [Header("Debug VIew")]
    public bool isSelected = false;
    public bool isHovered = false;

    [Header("Card Visuals")]
    public CardWobble cardWobble;
    public MaterialValueAdjust materialValue;

    // change this to however you see fit with the new animations
    private bool animate = false;

    void Awake()
    {
        hoverOutline.SetActive(false);
    }

    void Update()
    {
        // this needs to be changed if we want the cards to wait until the animation is finished
        if (card.shouldBeDiscarded) 
        {
            deckView.DespawnCard(this);
            return;
        }

        // change this to however you see fit with the new animations
        if (!animate)
        {
            return;
        }

    }
    public void Initialize()
    {
        // the code from deckView needs to be moved here

        EventManager.onCardSelectedEvent.AddListener(ProcessCardSelection);
    }

    void EnterSelected()
    {
        isSelected = true;
        materialValue.SetColor(Color.grey);
    }

    void ExitSelected()
    {
        isSelected = false;
        materialValue.SetColor(Color.black);
    }

    void ProcessCardSelection(CardObject selectedCardObject)
    {
        if(selectedCardObject == this)
        {
            if(!isSelected)
            {
                EnterSelected();
            }
        }
        else if (isSelected)
        {
            ExitSelected();
        }
    }

   public void OnStartHover()
    {
        if (isHovered) { return; }

        hoverOutline.SetActive(true);
        isHovered = true;
        cardWobble.SetHoverState(true);
    }

    public void OnStopHover()
    {
        if (!isHovered) { return; }

        hoverOutline.SetActive(false);
        isHovered = false;

        cardWobble.SetHoverState(false);
    }
}
