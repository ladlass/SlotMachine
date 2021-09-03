using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public abstract class ConditionBaseSO : ScriptableObject
    {
        public abstract bool IsConditionMet(int columnCount, int myColumnIndex, List<SlotSymbolTypes> symbols);

    }
}