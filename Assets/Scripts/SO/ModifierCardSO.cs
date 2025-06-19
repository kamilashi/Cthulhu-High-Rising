using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modifiers;


[CreateAssetMenu(fileName = "ModifierCard", menuName = "ScriptableObjects/ModifierCard", order = 5)]
public class ModifierCardSO : CardSO
{
    public ModifierData modifierData;

    [Header("Make sure to put in whole numbers for int typed properties!")]
    public float operand;
    public ModifierCardSO()
    {
        cardType = CardType.Modifier;
    }
}