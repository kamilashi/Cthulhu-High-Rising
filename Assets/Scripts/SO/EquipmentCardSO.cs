using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentCard", menuName = "ScriptableObjects/EquipmentCard", order = 4)]
public class EquipmentCardSO : CardSO
{
    // public EquipmentSO equipmentSO;
    public EquipmentCardSO()
    {
        cardType = CardType.Equipment;
    }
}
