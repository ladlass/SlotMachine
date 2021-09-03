using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SlotMachine;
public class RandomGeneratorTest
{
    
    [Test]
    public void Test100RandomSequenceAndTheirIntervals()
    {
        List<SymbolSequenceProbabilityData> sequences = new List<SymbolSequenceProbabilityData>();
        List<SlotSymbolTypes> symbols = new List<SlotSymbolTypes>();
        symbols.Add(SlotSymbolTypes.A);
        symbols.Add(SlotSymbolTypes.A);
        symbols.Add(SlotSymbolTypes.A);


        int totalPercentage = 100;
        while (totalPercentage > 0)
        {
            int ceiling = Mathf.Min(15, totalPercentage);

            int randomPerc = Random.Range(1, ceiling);

            SymbolSequenceProbabilityData newSeq = new SymbolSequenceProbabilityData();
            newSeq.percentage = randomPerc;
            newSeq.symbols = symbols;
            sequences.Add(newSeq);

            totalPercentage -= randomPerc;
        }

        SlotRandomManager randomManager = new SlotRandomManager(sequences);

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
            Assert.IsTrue(randomSeqDat[i].floor <= i && randomSeqDat[i].ceiling >= i);
        }

        for (int i = 0; i < intervalLog.Count; i++)
        {
            Debug.Log(intervalLog[i]);
        }
        Debug.Log("--------------------");
    }
}
