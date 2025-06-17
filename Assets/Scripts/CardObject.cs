using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [Header("Setup")]
    //- this will change when we have separate prefabs for different card types.
    // Once we do, each CardObject might have to handle card display info differently.
    public TextMeshPro nameText;
    public TextMeshPro descriptionText;

    [Header("AutoSetup")]
    public DeckView deckView;
    public Card card;

    void Initialize()
    {
        // the code from deckView needs to be moved here
    }

    public void OnCardClicked()
    {
        deckView.OnCardClicked(this);
    }
}
