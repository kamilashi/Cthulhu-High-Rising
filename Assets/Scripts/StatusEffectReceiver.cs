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

    

    void Awake()
    {
        Debug.Assert(receiverRigidbody != null);
        Debug.Assert(receiverCollider != null);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessStatusEffects();
    }

    public void AddOrUpdateStatusEffect(StatusEffectTemplate effectTemplate)
    {
        StatusEffect effect;

        if (activeStatusEffects.ContainsKey(effectTemplate.effectType))
        {
            effect = activeStatusEffects[effectTemplate.effectType];
            effect.isActiveThisFrame = true;
        }
        else
        {
            StatusEffect newEffect = new StatusEffect(effectTemplate);
            activeStatusEffects.Add(effectTemplate.effectType, newEffect);
            effect = newEffect;
        }

        ProcessEffectInitialization(effect);
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

        effect.isActiveThisFrame = false;
    }

    private void ProcessEffectInitialization(StatusEffect effect)
    {
        if(effect.effectType == StatusEffectType.Slowdown)
        {
            effect.SetModifiableFieldRefernce(enemyControler.modifiableMovementSpeed);
            effect.modifiableFloatFieldReference.AddModifier(effect.floatOperation);
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
        if (!effect.isActiveThisFrame)
        {
            effect.modifiableFloatFieldReference.AddModifier(effect.floatOperation);
            RemoveActiveStatusEffect(effect.effectType);
            Debug.Log("Removed slowed status effect");
            return;
        }
    }

    private void ProcessDamageOverTime(StatusEffect effect)
    {
        if (!effect.isActiveThisFrame)
        {
            if(effect.floatOperation != null)
            {
                effect.modifiableFloatFieldReference.RemoveModifier(effect.floatOperation);
            }

            dotTimer = -1.0f;
            RemoveActiveStatusEffect(effect.effectType);
            Debug.Log("Stopped DoT");
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
