using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers 
{

    public enum ModifyOperation
    {
        Multiply,
        Add,
        Subtract
    }

/*
    [Serializable]
    public class ModifyOperation<T>
    {
        public ModifyOperation operation;
        public T Operand;

        public T Apply(T BaseValue) // to be read from in runtime
        {
            return BaseValue;
        }

        private void Operate(T baseOpnd, ModifyOperation optn, T secondOpnd)
        {
            switch(optn)
            {
                case ModifyOperation.Multiply:
                    return baseOpnd * secondOpnd;

            }
        }
    }

    [Serializable]
    public class ModifiableData<T>
    {
        public T baseValue;
        public T modifiedValue;
        public List<ModifyOperation> operations;

        public T GetBadeValue() // to be read from in runtime
        {
            return baseValue;
        }
        public T GetAndStoreValue() // to be read from in runtime
        {
            static T modifiedValue = 
            return baseValue;
        }

        private T ApplyAllOperations(T baseV)
        {
            T modifiedV = baseV;
            for (int i = 0; i < operations.Count; i++)
            {
                modifiedV = operations[i].
            }
        }
    }*/
}
