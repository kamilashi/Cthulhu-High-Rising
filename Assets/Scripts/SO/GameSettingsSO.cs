using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CardCountInDeck <DerivedCardType>
{
    [SerializeReference]
    public DerivedCardType cardSO;
    //public CardSO cardSO;
    public int count;
}


[Serializable]
public class DeckData
{
    public List<CardCountInDeck<BlockCardSO>> blockCardCounts;
    public List<CardCountInDeck<EquipmentCardSO>> equipmentCardCounts;
    public List<CardCountInDeck<ModifierCardSO>> modifierCardCounts;

   // public List<CardCountInDeck> cardCounts;
}

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettingsSO : ScriptableObject
{
    public DeckData defaultCardDeck;
}
