using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Modifiers;

public enum StatusEffectType
{
    Slowdown,
    DoTEverySecond
}

[Serializable]
public class StatusEffectTemplate
{
    public StatusEffectType effectType;

    public int damageOverTimeValue;
    public float slowDownFactor;

    public ModifyOperation<ModifiableFloat> floatOperation;
}

[Serializable]
public class StatusEffect : StatusEffectTemplate
{
    public bool isActive;
    public ModifiableData<ModifiableFloat> modifiableFloatFieldReference;

    public StatusEffect(StatusEffectTemplate template)
    {
        effectType = template.effectType;
        damageOverTimeValue = template.damageOverTimeValue;
        slowDownFactor = template.slowDownFactor;

        floatOperation = new ModifyOperation<ModifiableFloat>(template.floatOperation.operation, slowDownFactor);

        isActive = true;   
    }

    public void SetModifiableFieldRefernce(ModifiableData<ModifiableFloat> fieldToModify)
    {
        modifiableFloatFieldReference = fieldToModify;
    }
}

public class StatusEffectApplier : MonoBehaviour
{
    [Header("Setup in Prefab")]
    public Collider triggerCollider;
    public LayerMask receiverMask;
    public StatusEffectTemplate appliedStatusEffect; // so far one per applier, but might change

    //[Header("Debug View")]

    void Awake()
    {
        Debug.Assert(triggerCollider != null);
    }

    void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & receiverMask) != 0)
        {
            StatusEffectReceiver receiver = other.gameObject.GetComponentInChildren<StatusEffectReceiver>();
            receiver.AddOrUpdateStatusEffect(appliedStatusEffect);
        }
    }
}
