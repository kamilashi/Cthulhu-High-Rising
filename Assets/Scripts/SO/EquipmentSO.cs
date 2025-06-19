using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment", order = 6)]
public class EquipmentSO : ScriptableObject
{
    public GameObject prefab;

    public BaseEquipmentData baseData;

    public string givenName;
}
