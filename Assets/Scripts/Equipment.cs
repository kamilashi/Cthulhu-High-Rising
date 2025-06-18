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

    public string givenName;
}

public class Equipment : MonoBehaviour, IHoverable
{
    public EquipmentData equipmentData;
    public Block blockOwner;

    void IHoverable.OnStartHover()
    {
        throw new NotImplementedException();
    }

    void IHoverable.OnStopHover()
    {
        throw new NotImplementedException();
    }
}
