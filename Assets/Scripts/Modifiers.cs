using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Modifiers 
{
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

            Debug.LogWarning("Unsupported modifiable type " + baseValue.GetType().Name + "!");
        }

        private void Operate(ref T baseOpnd, ModifyOperationType optn, object secondOpnd)
        {
            switch(optn)
            {
                case ModifyOperationType.Multiply:
                    {
                        baseOpnd.Multiply(secondOpnd);
                        break;
                    }
                case ModifyOperationType.Add:
                    {
                        baseOpnd.Add(secondOpnd);
                        break;
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

        public object GetBaseValue() // to be read from in runtime
        {
            return baseValueContainer.GetValue();
        }
        public object GetAndStoreValue() // to be read from in runtime
        {
            if(newRevision != oldRevision)
            {
                modifiedValueContainer = ApplyAllOperations(baseValueContainer);
                oldRevision = newRevision;
            }

            return modifiedValueContainer.GetValue();
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
        ModifyOperation<ModifiableFloat> damageModifyOperation = new ModifyOperation<ModifiableFloat>(modifierData.operation, operand);

        switch (modifierData.modifiablePropertyType)
        {
            case ModifiablePropertyType.EquipmentDamage:
                {
                    target.equipmentData.damage.AddModifier(damageModifyOperation);
                    break;
                }
            case ModifiablePropertyType.EquipmentRange:
                {
                    target.equipmentData.attackRange.AddModifier(damageModifyOperation);
                    break;
                }
            case ModifiablePropertyType.EquipmentSpeed:
                {
                    target.equipmentData.attackSpeed.AddModifier(damageModifyOperation);
                    break;
                }
        }
    }
}
