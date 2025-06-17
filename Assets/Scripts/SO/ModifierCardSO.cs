using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ModifierCard", menuName = "ScriptableObjects/ModifierCard", order = 5)]
public class ModifierCardSO : CardSO
{
    public ModifierTarget modifierTarget;
    // parameter to modify
    // modify operation
    // modify operand 2

    public ModifierCardSO()
    {
        cardType = CardType.Modifier;
    }
}