using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [System.Serializable]
    public class SpinnerConditionHelper
    {
        private bool isSelectionMade = false;
        private bool isConditionTrue = false;


        public ConditionPrevSymbolsEqualSO condition;
        public SpinnerSO spinnerOnTrue;
        public SpinnerSO spinnerOnFalse;

        public SpinnerConditionHelper(SpinnerConditionHelper clone)
        {
            if (clone == null) return;

            if (clone.condition)
            {
                condition = Object.Instantiate(clone.condition);
            }
            if (clone.spinnerOnTrue)
            {
                spinnerOnTrue = Object.Instantiate(clone.spinnerOnTrue);
            }
            if (clone.spinnerOnFalse)
            {
                spinnerOnFalse = Object.Instantiate(clone.spinnerOnFalse);
            }
        }

        public void ResetSelection()
        {
            isSelectionMade = false;
        }

        public SpinnerSO SelectSpinnerBasedOnCondition(int columnCount, int myColumnIndex, List<SlotSymbolTypes> symbols)
        {
            isSelectionMade = true;

            if (condition == null)
            {
                isConditionTrue = true;
                return spinnerOnTrue;
            }
            else
            {
                if (condition.IsConditionMet(columnCount, myColumnIndex, symbols))
                {
                    isConditionTrue = true;

                    return spinnerOnTrue;
                }
                else
                {
                    isConditionTrue = false;

                    return spinnerOnFalse;
                }
            }
        }

        public SpinnerSO GetSelectedSpinner()
        {
            if (isSelectionMade)
            {
                if (isConditionTrue)
                {
                    return spinnerOnTrue;
                }
                else
                {
                    return spinnerOnFalse;
                }
            }

            return null;
        }
    }
}