using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SlotRandomizationTestUtil : MonoBehaviour
    {
        public SlotSequencesSO sequence;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TestInvervals();
            }
        }

        void TestInvervals()
        {
            if (sequence)
            {
                SlotRandomManager randomManager = new SlotRandomManager(sequence.sequences);
                randomManager.Calculate100RandomSequence();
                List<RandomSequenceData> randomSeqDat = randomManager.GetRandSeqData();

                Debug.Log("----------100 Random Sequence----------");
                List<string> intervalLog = new List<string>();
                for (int i = 0; i < randomManager.GetSequencePreset().Count; i++)
                {
                    intervalLog.Add(string.Format("Percentage: {0}, intervals = ", randomManager.GetSequencePreset()[i].percentage));
                }

                for (int i = 0; i < randomSeqDat.Count; i++)
                {
                    intervalLog[randomSeqDat[i].sequenceIndex] += string.Format(" [[{0}, {1}] = {2}] ", randomSeqDat[i].floor, randomSeqDat[i].ceiling, i);
                }

                for (int i = 0; i < intervalLog.Count; i++)
                {
                    Debug.Log(intervalLog[i]);
                }
                Debug.Log("--------------------");
            }
        }
    }
}