using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [Serializable]
    public class SymbolSequenceProbabilityData : IEquatable<SymbolSequenceProbabilityData>, IComparable<SymbolSequenceProbabilityData>
    {
        [Range(1, 100)]
        public int percentage;
        public List<SlotSymbolTypes> symbols;
        public bool spawnFxOnFinish = false;
        [Range(1, 10)]
        public int fxAmountMultiplier = 1;

        public SymbolSequenceProbabilityData()
        { 
        
        }

        public SymbolSequenceProbabilityData(SymbolSequenceProbabilityData clone)
        {
            percentage = clone.percentage;
            symbols = new List<SlotSymbolTypes>(clone.symbols);
            spawnFxOnFinish = clone.spawnFxOnFinish;
            fxAmountMultiplier = clone.fxAmountMultiplier;
        }
        public int CompareTo(SymbolSequenceProbabilityData comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return percentage.CompareTo(comparePart.percentage) * -1;//-1 for descending order
        }

        public bool Equals(SymbolSequenceProbabilityData other)
        {
            if (other == null) return false;
            return percentage.Equals(other.percentage);
        }
    }
}