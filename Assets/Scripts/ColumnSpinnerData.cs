using System.Collections.Generic;

namespace SlotMachine
{
    [System.Serializable]
    public class ColumnSpinnerData
    {
        public SlotColumn column;
        public List<SpinnerConditionHelper> spinnersWithCondition;
    }
}