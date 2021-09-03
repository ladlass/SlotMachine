using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "PrevSymbolsEqual", menuName = "ScriptableObjects/SlotCondition", order = 4)]
    public class ConditionPrevSymbolsEqualSO : ConditionBaseSO
    {
        public override bool IsConditionMet(int columnCount, int myColumnIndex, List<SlotSymbolTypes> symbols)
        {
            if (symbols == null) return false;
            if (myColumnIndex >= symbols.Count || columnCount != symbols.Count || symbols.Count < 3 || myColumnIndex < 2) return false;

            bool isConditionsMet = true;
            for (int i = 1; i < symbols.Count && i < myColumnIndex; i++)
            {
                if (symbols[i - 1] != symbols[i])
                {
                    isConditionsMet = false;
                    break;
                }
            }
            return isConditionsMet;
        }
    }
}