using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpinnerBrain : MonoBehaviour
{
    [SerializeField]private List<ColumnSpinnerHelper> columnData;

    [SerializeField]private SymbolPrefabsSO prefabsList;
    [SerializeField]private SlotSequencesSO slotSequence;
    [SerializeField]private Transform garbage;

    [SerializeField] [Range(0, 0.8f)]private float eachColumnSpinStartDelayDurationMin;
    [SerializeField] [Range(0, 0.8f)]private float eachColumnSpinStartDelayDurationMax;

    [SerializeField] float imageVerticalSize = 260;

    private SlotSymbolPool symbolPool;
    private bool enableSpinning = false;

    private SlotRandomManager slotRandManager;
    private SaveActions saveActions;
    private void Awake()
    {
        symbolPool = new SlotSymbolPool(prefabsList.CreateDictionary(), garbage, prefabsList.prefabClass);
        PrepareDependencies();
        AwakenSpinners();

        saveActions = new SaveActions();
        SaveData savedData = saveActions.LoadGame();

        if (savedData != null)
        {
            slotRandManager = new SlotRandomManager(slotSequence.sequences, savedData.randomSeqData, savedData.currentRandomSeqIndex);
        }
        else
        {
            slotRandManager = new SlotRandomManager(slotSequence.sequences);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        enableSpinning = true;
        //for (int i = 0; i < 102; i++)
        //{
        //    RandomSequenceData dat = slotRandManager.PullRandomSequence();
        //    Debug.Log(string.Format("Index {0}, seq index{1}", dat.randomSeqIndex, dat.sequenceIndex));
        //}
    }

    private void PrepareDependencies()
    {
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column)
            {
                columnData[i].column.SetPool(symbolPool);

                SpinnerManager spinnerManager = new SpinnerManager(imageVerticalSize, columnData.Count, i);

                spinnerManager.AddSpinner(columnData[i].spinnersWithCondition);
                columnData[i].column.SetSpinnerManager(spinnerManager);
            }
        }
    }

    private void AwakenSpinners()
    {
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column)
            {
                columnData[i].column.AwakeSpinner(imageVerticalSize);
            }
        }
    }

    private void UpdateSpinners()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column)
            {
                columnData[i].column.UpdateSpinner(deltaTime);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartSpinning();
        }

        UpdateSpinners();
    }

    void StartSpinning()
    {
        if (enableSpinning == false) return;

        //Do not enable spinning until the previous spin stops
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column.IsSpinning == true)
            {
                return;
            }
        }
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        List<SlotSymbolTypes> randomSeq = slotRandManager.PullRandomSequence();

        if (randomSeq.Count != columnData.Count) yield break;

        for (int i = 0; i < columnData.Count && i < randomSeq.Count; i++)
        {
            if (columnData[i].column)
            {
                float delay = Random.Range(eachColumnSpinStartDelayDurationMin, eachColumnSpinStartDelayDurationMax);
                columnData[i].column.StartSpinning(randomSeq[i], randomSeq);

                yield return new WaitForSeconds(delay);
            }
        }
    }

}

[System.Serializable]
public class ColumnSpinnerHelper
{
    public SlotColumn column;
    public List<SpinnerConditionData> spinnersWithCondition;
}

[System.Serializable]
public class SpinnerConditionData
{
    private bool isSelectionMade = false;
    private bool isConditionTrue = false;
   

    public ConditionPrevSymbolsEqualSO condition;
    public SpinnerSO spinnerOnTrue;
    public SpinnerSO spinnerOnFalse;

    public SpinnerConditionData(SpinnerConditionData clone)
    {
        if (clone == null) return;

        if (clone.condition)
        {
            condition = ScriptableObject.Instantiate(clone.condition);
        }
        if (clone.spinnerOnTrue)
        {
            spinnerOnTrue = ScriptableObject.Instantiate(clone.spinnerOnTrue);
        }
        if (clone.spinnerOnFalse)
        {
            spinnerOnFalse = ScriptableObject.Instantiate(clone.spinnerOnFalse);
        }
    }

    public void ResetSelection()
    {
        isSelectionMade = false;
    }

    public SpinnerSO SelectSpinnerBasedOnCondition(int columnCount, int myColumnIndex, List<SlotSymbolTypes> symbols)
    {
        isSelectionMade = true;

        if (condition == null)
        {
            isConditionTrue = true;
            return spinnerOnTrue;
        }
        else
        {
            if(condition.IsConditionMet(columnCount, myColumnIndex, symbols))
            {
                isConditionTrue = true;

                return spinnerOnTrue;
            }
            else
            {
                isConditionTrue = false;

                return spinnerOnFalse;
            }
        }
    }

    public SpinnerSO GetSelectedSpinner()
    {
        if (isSelectionMade)
        {
            if (isConditionTrue)
            {
                return spinnerOnTrue;
            }
            else
            {
                return spinnerOnFalse;
            }
        }

        return null;
    }

}
