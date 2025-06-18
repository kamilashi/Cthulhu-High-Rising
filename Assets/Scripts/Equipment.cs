using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EquipmentData
{
    public ModifiableData Damage;
    public ModifiableData AttackRange;
    public ModifiableData AttackSpeed;
}

public class Equipment : MonoBehaviour
{
    public EquipmentData equipmentData;
    public Block blockOwner;

}
