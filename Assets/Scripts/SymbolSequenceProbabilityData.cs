using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolSequenceProbabilityData : IEquatable<SymbolSequenceProbabilityData>, IComparable<SymbolSequenceProbabilityData>
{
    [Range(1, 100)]
    public int percentage;
    public List<SlotSymbolTypes> symbols;

    public SymbolSequenceProbabilityData(SymbolSequenceProbabilityData clone)
    {
        this.percentage = clone.percentage;
        this.symbols = new List<SlotSymbolTypes>(clone.symbols);

    }
    public int CompareTo(SymbolSequenceProbabilityData comparePart)
    {
        // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            return this.percentage.CompareTo(comparePart.percentage) * -1;//-1 for descending order
    }
   
    public bool Equals(SymbolSequenceProbabilityData other)
    {
        if (other == null) return false;
        return (this.percentage.Equals(other.percentage));
    }
}