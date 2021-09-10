using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SlotRandomManagerComponent : MonoBehaviour
    {
        [SerializeField] private SlotSequencesSO slotSequence;
        private SlotRandomManager slotRandManager;
        public void AwakeComponent(SaveManagerComponent saveManager)
        {
            if (slotSequence == null)
            {
                new Exception("No Sequence Selected");
                return;
            }

            SaveData savedData = saveManager.LoadGame();

            if (savedData != null)
            {
                slotRandManager = new SlotRandomManager(slotSequence.sequences, savedData.randomSeqData, savedData.currentRandomSeqIndex);
            }
            else
            {
                slotRandManager = new SlotRandomManager(slotSequence.sequences);
            }
        }

        public List<RandomSequenceData> GetRandSeqData()
        {
            return slotRandManager.GetRandSeqData();
        }

        public int GetCurrentIndex()
        {
            return slotRandManager.GetCurrentIndex();
        }

        public SymbolSequenceProbabilityData PullRandomSequence()
        {
            if (slotRandManager == null) return null;

            return slotRandManager.PullRandomSequence();
        }
    }
}