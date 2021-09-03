using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [System.Serializable]
    public class SaveData
    {
        public int currentRandomSeqIndex;
        public List<RandomSequenceData> randomSeqData;
    }
}