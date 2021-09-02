using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/SlotSequence", order = 3)]
public class SlotSequencesSO : ScriptableObject
{
    public List<SymbolSequenceProbabilityData> sequences;
}
