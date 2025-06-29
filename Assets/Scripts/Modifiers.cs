using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Modifiers
{
    public static Type GetModifierType(ModifiablePropertyType propertyType)
    {
        switch(propertyType)
        {
            case ModifiablePropertyType.EquipmentDamage:
                {
                    return typeof(int);
                }
            case ModifiablePropertyType.EquipmentSpeed:
                {
                    return typeof(float);
                }
            case ModifiablePropertyType.EquipmentRange:
                {
                    return typeof(float);
                }
        }

        Debug.LogError("Unknown property type! Please include it in the switch case");
        return typeof(float);
    }
    public enum ModifiablePropertyType
    {
        NONE_BlockModCount,

        EquipmentDamage,
        EquipmentSpeed,
        EquipmentRange,

        NONE_EquipmentModCount
    }

    [Serializable]
    public struct ModifierData
    {
        public ModifierTarget target;
        public ModifiablePropertyType modifiablePropertyType;
        public ModifyOperationType operation;
    }

    public enum ModifyOperationType
    {
        Multiply,
        Add
    }

    public interface INumeric
    {
        void Add(object value);
        void Multiply(object value);
        object GetValue();
    }

    [Serializable]
    public struct ModifiableInt : INumeric
    {
        public int value;

        public void Add(object second)
        {
            value += (int)second;
        }

        public void Multiply(object second)
        {
            value *= (int)second;
        }

        public object GetValue()
        {
            return value;   
        }
    }

    [Serializable]
    public struct ModifiableFloat : INumeric
    {
        public float value;

        public void Add(object second)
        {
            value += (float)second;
        }

        public void Multiply(object second)
        {
            value *= (float)second;
        }
        public object GetValue()
        {
            return value;
        }
    }


    [Serializable]
    public class ModifyOperation<T> where T : INumeric
    {
        public ModifyOperationType operation;
        public object operand;

        public ModifyOperation(ModifyOperationType op, object opnd)
        {
            operation = op;
            operand = opnd;
        }
        public void Apply(ref T baseValue) // to be read from in runtime
        {
            Operate(ref baseValue, operation, operand);

            Debug.Assert(baseValue.GetValue().GetType() == operand.GetType(), "The types of the operands should be the same!");
        }

        private void Operate(ref T baseOpnd, ModifyOperationType optn, object secondOpnd)
        {
            switch(optn)
            {
                case ModifyOperationType.Multiply:
                    {
                        baseOpnd.Multiply(secondOpnd);
                        return;
                    }
                case ModifyOperationType.Add:
                    {
                        baseOpnd.Add(secondOpnd);
                        return;
                    }
            }

            Debug.LogWarning(optn.ToString() + " operation failed! ");
        }
    }

    [Serializable]
    public class ModifiableData<T> where T : INumeric
    {
        public T baseValueContainer;
        public T modifiedValueContainer;
        public List<ModifyOperation<T>> operations = new List<ModifyOperation<T>>();

        private int newRevision = 0;
        private int oldRevision = -1;

        public object GetBaseValue()
        {
            return baseValueContainer.GetValue();
        }
        public object GetAndStoreValue() 
        {
            if(newRevision != oldRevision)
            {
                modifiedValueContainer = ApplyAllOperations(baseValueContainer);
                oldRevision = newRevision;
            }

            return modifiedValueContainer.GetValue();
        }
        public TNumeric GetAndStoreValue<TNumeric>() 
        {
            return (TNumeric) GetAndStoreValue();
        }

        private T ApplyAllOperations(T baseV)
        {
            T modifiedV = baseV;
            for (int i = 0; i < operations.Count; i++)
            {
                operations[i].Apply(ref modifiedV);
            }

            return modifiedV;
        }

        public void AddModifier(ModifyOperation<T> newOperation)
        {
            operations.Add(newOperation);
            newRevision++;
        }

        public void ClearAllModifiers()
        {
            operations.Clear();
            newRevision++;
        }
        public void RemoveModifier(ModifyOperation<T> operationReference)
        {
            operations.Remove(operationReference);
            newRevision++;
        }
    }

    public static void ModifyTarget(Block target, ModifierData modifierData, object operand)
    {
        Debug.Assert(modifierData.modifiablePropertyType < ModifiablePropertyType.NONE_EquipmentModCount, "Unsupported modifiable property for " + target.GetType().Name + "!");
    }
    public static void ModifyTarget(Equipment target, ModifierData modifierData, object operand)
    {
        bool canContinue = modifierData.modifiablePropertyType > ModifiablePropertyType.NONE_BlockModCount && modifierData.modifiablePropertyType < ModifiablePropertyType.NONE_EquipmentModCount;

        if(!canContinue)
        {
            Debug.LogAssertion("Unsupported modifiable property for " + target.GetType().Name + "!");
            return;
        }

        switch (modifierData.modifiablePropertyType)
        {
            case ModifiablePropertyType.EquipmentDamage:
                {
                    ModifyOperation<ModifiableInt> modifyOperation = new ModifyOperation<ModifiableInt>(modifierData.operation, operand);
                    target.equipmentData.damage.AddModifier(modifyOperation);
                    break;
                }
            case ModifiablePropertyType.EquipmentRange:
                {
                    ModifyOperation<ModifiableFloat> modifyOperation = new ModifyOperation<ModifiableFloat>(modifierData.operation, operand);
                    target.equipmentData.attackRange.AddModifier(modifyOperation);
                    break;
                }
            case ModifiablePropertyType.EquipmentSpeed:
                {
                    ModifyOperation<ModifiableFloat> modifyOperation = new ModifyOperation<ModifiableFloat>(modifierData.operation, operand);
                    target.equipmentData.attackSpeed.AddModifier(modifyOperation);
                    break;
                }
        }
    }
}
