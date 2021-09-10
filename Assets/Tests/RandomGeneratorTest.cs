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

    [Test]
    public void Test100SequenceAndTheirIntervals()
    {
        List<SymbolSequenceProbabilityData> sequences = new List<SymbolSequenceProbabilityData>();
        
        //%13 
        List<SlotSymbolTypes> symbols = new List<SlotSymbolTypes>();
        symbols.Add(SlotSymbolTypes.Bonus);
        symbols.Add(SlotSymbolTypes.A);
        symbols.Add(SlotSymbolTypes.Jackpot);

        SymbolSequenceProbabilityData newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 13;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 13;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 13;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 13;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 13;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

        //%9
        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 9;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);
        //%8
        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 8;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);
        //%7
        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 7;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);
        //%6
        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 6;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);
        //%5
        newSeq = new SymbolSequenceProbabilityData();
        newSeq.percentage = 5;
        newSeq.symbols = symbols;
        sequences.Add(newSeq);

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


    [Test]
    public void TestSpinnerManagerSpinTimer()
    {
        SpinnerSO spinner = (SpinnerSO)(ScriptableObject.CreateInstance(nameof(SpinnerSO)));
        spinner.duration = 1;
        spinner.speedMult = 5;
        spinner.easeType = EaseTypes.Lineer;

        SpinnerConditionHelper condHelper = new SpinnerConditionHelper();
        condHelper.condition = null;
        condHelper.spinnerOnTrue = spinner;

        List<SpinnerConditionHelper> spinnerConditionHelper = new List<SpinnerConditionHelper>();
        spinnerConditionHelper.Add(condHelper);

        SpinnerManager spinnerManager = new SpinnerManager(200, 3, 2, spinnerConditionHelper);
        spinnerManager.Reset(null);


        spinnerManager.Spin(0.99f);


        Assert.IsFalse(spinnerManager.IsSpinningEnded());

        spinnerManager.Spin(0.01f);

        Assert.IsTrue(spinnerManager.IsSpinningEnded());
    }


    [Test]
    public void TestSpinnerTimer()
    {
        SpinnerSO spinner = (SpinnerSO)(ScriptableObject.CreateInstance(nameof(SpinnerSO)));
        spinner.duration = 1;
        spinner.speedMult = 5;
        spinner.easeType = EaseTypes.Lineer;

        spinner.UpdatePosition(0.99f);

        Assert.IsFalse(spinner.IsTimeUp());

        spinner.UpdatePosition(0.01f);

        Assert.IsTrue(spinner.IsTimeUp());
    }

    [Test]
    public void TestSpinHandlerEventsAndTimer()
    {
        SpinnerSO spinner = (SpinnerSO)(ScriptableObject.CreateInstance(nameof(SpinnerSO)));
        spinner.duration = 1;
        spinner.speedMult = 5;
        spinner.easeType = EaseTypes.Lineer;

        SpinnerConditionHelper condHelper = new SpinnerConditionHelper();
        condHelper.condition = null;
        condHelper.spinnerOnTrue = spinner;

        List<SpinnerConditionHelper> spinnerConditionHelper = new List<SpinnerConditionHelper>();
        spinnerConditionHelper.Add(condHelper);

        const float verticalSizeOfImage = 200;
        SpinnerManager spinnerManager = new SpinnerManager(verticalSizeOfImage, 3, 2, spinnerConditionHelper);

        List<SlotSymbolData> slotSymbols = new List<SlotSymbolData>();

        float topPoint = -verticalSizeOfImage;
        for (int i = 0; i < 3; i++)
        {
            SlotSymbolData newSymbol = new SlotSymbolData();
            GameObject newObj = new GameObject();
            SlotSymbolGameObj slotGameObj = newObj.AddComponent<SlotSymbolGameObj>();
            newSymbol.rect = newObj.GetComponent<RectTransform>();
            newSymbol.slotGameObj = slotGameObj;
            newSymbol.rect.anchoredPosition = new Vector2(0, topPoint);
            topPoint += verticalSizeOfImage;
            slotSymbols.Add(newSymbol);
        }
        

        bool didOnThresholdCalled = false;
        bool didOnBlurCalled = false;

        SpinHandler spinHandler = new SpinHandler(spinnerManager, slotSymbols, 
        (int index, bool b) => 
        {
            int topIndex = index - 1 >= 0 ? index - 1: slotSymbols.Count - 1;
            slotSymbols[index].rect.anchoredPosition = new Vector2(0, slotSymbols[topIndex].rect.anchoredPosition.y);
            didOnThresholdCalled = true; 
        }, 
        (bool b) => 
        { 
            didOnBlurCalled = true; 
        }, -400);

        spinHandler.StartSpinning(null);

        for (float i = 0f; i < 0.9f; i+= Time.deltaTime)
        {
            spinHandler.UpdateSpinner(Time.deltaTime);
        }

        Assert.IsTrue(spinHandler.IsSpinning);

        spinHandler.UpdateSpinner(0.1f);

        Assert.IsFalse(spinHandler.IsSpinning);
        Assert.IsTrue(didOnThresholdCalled);
        Assert.IsTrue(didOnBlurCalled);
    }
}
