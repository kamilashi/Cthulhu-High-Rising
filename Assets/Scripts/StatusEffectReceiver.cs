using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectReceiver : MonoBehaviour
{
    [Header("Setup in Prefab")]
    public Rigidbody receiverRigidbody;
    public Collider receiverCollider;

    public EnemyHealth enemyHealth;
    public EnemyController enemyControler;

    [Header("Debug View")]
    public float dotTimer = -1.0f; // temporary hack

    public Dictionary<StatusEffectType, StatusEffect> activeStatusEffects = new Dictionary<StatusEffectType, StatusEffect>();
    public List<StatusEffect> newEffects = new List<StatusEffect>();
    public List<StatusEffect> inactiveEffects = new List<StatusEffect>();
    

    void Awake()
    {
        Debug.Assert(receiverRigidbody != null);
        Debug.Assert(receiverCollider != null);
    }

    void FixedUpdate()
    {
        CommitInactiveEffects();
        CommitActiveEffects();
        ProcessStatusEffects();
        RemoveInactiveEffects();
    }

    public void AddOrUpdateStatusEffect(StatusEffectTemplate effectTemplate)
    {
        if (activeStatusEffects.ContainsKey(effectTemplate.effectType))
        {
            StatusEffect effect = activeStatusEffects[effectTemplate.effectType];
            effect.isActive = true;
        }
        else
        {
            StatusEffect newEffect = new StatusEffect(effectTemplate);
            newEffects.Add(newEffect);

            ProcessEffectInitialization(newEffect);
        }
    }

    private void ProcessStatusEffects()
    {
        foreach (StatusEffect effect in activeStatusEffects.Values)
        {
            ProcessStatusEffect(effect);
        }
    }

    private void ProcessStatusEffect(StatusEffect effect)
    {
        switch(effect.effectType)
        {
            case StatusEffectType.Slowdown:
                ProcessSlowDown(effect);
                break;

            case StatusEffectType.DoTEverySecond:
                ProcessDamageOverTime(effect);
                break;
        }

        effect.isActive = false;
    }

    private void CommitActiveEffects()
    {
        foreach (StatusEffect effect in newEffects)
        {
            activeStatusEffects.Add(effect.effectType, effect);
        }
        newEffects.Clear();
    }

    private void CommitInactiveEffects()
    {
        foreach (StatusEffect effect in activeStatusEffects.Values)
        {
            if(!effect.isActive)
            {
                inactiveEffects.Add(effect);
                Debug.Log("Effect in queue for removal " + effect.effectType.ToString());
            }
        }
    }

    private void RemoveInactiveEffects()
    {
        foreach(StatusEffect effect in inactiveEffects)
        {
            Debug.Log("Removed status effect: " + effect.effectType.ToString());
            RemoveActiveStatusEffect(effect.effectType);
        }
        inactiveEffects.Clear();
    }

    private void ProcessEffectInitialization(StatusEffect effect)
    {
        if(effect.effectType == StatusEffectType.Slowdown)
        {
            effect.SetModifiableFieldRefernce(enemyControler.modifiableMovementSpeed);
            effect.modifiableFloatFieldReference.AddModifier(effect.floatOperation);

            //effect.floatOperation.Apply(ref enemyControler.modifiableMovementSpeed.baseValueContainer);

            Debug.Log("Added slowed status effect");
        }
        else if(effect.effectType == StatusEffectType.DoTEverySecond)
        {
            dotTimer = 0.0f;
            Debug.Log("Started DoT");
        }
    }

    private void ProcessSlowDown(StatusEffect effect)
    {
        if (!effect.isActive)
        {
            if(effect.modifiableFloatFieldReference != null) // this check should not be needed, because if the carrier is despawned this script will be as well
            {
                effect.modifiableFloatFieldReference.RemoveModifier(effect.floatOperation);
            }
            return;
        }
    }

    private void ProcessDamageOverTime(StatusEffect effect)
    {
        if (!effect.isActive)
        {
            dotTimer = -1.0f;
            return;
        }

        if (dotTimer <= 0)
        {
            enemyHealth.getHit(effect.damageOverTimeValue);
            dotTimer += 1.0f;
        }

        dotTimer -= Time.deltaTime;
    }

    private void RemoveActiveStatusEffect(StatusEffectType effectTypeKey)
    {
        activeStatusEffects.Remove(effectTypeKey);
    }
}
