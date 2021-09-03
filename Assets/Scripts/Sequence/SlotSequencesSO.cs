using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/SlotSequence", order = 3)]
    public class SlotSequencesSO : ScriptableObject
    {
        public List<SymbolSequenceProbabilityData> sequences;
    }
}