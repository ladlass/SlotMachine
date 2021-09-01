using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpinnerBrain : MonoBehaviour
{
    [SerializeField]private List<ColumnSpinnerHelper> columnData;

    [SerializeField]private SymbolPrefabsSO prefabsList;
    [SerializeField]private Transform garbage;

    [SerializeField] [Range(0, 0.8f)]private float eachColumnSpinStartDelayDurationMin;
    [SerializeField] [Range(0, 0.8f)]private float eachColumnSpinStartDelayDurationMax;

    [SerializeField] float imageVerticalSize = 260;

    private SlotSymbolPool symbolPool;
    private bool enableSpinning = false;

    private void Awake()
    {
        symbolPool = new SlotSymbolPool(prefabsList.CreateDictionary(), garbage);
        PrepareDependencies();
        AwakenSpinners();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        enableSpinning = true;
    }
    private void PrepareDependencies()
    {
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column)
            {
                columnData[i].column.SetPool(symbolPool);

                SpinnerManager spinnerManager = new SpinnerManager(imageVerticalSize);

                for (int j = 0; j < columnData[i].spinner.Count; j++)
                {
                    spinnerManager.AddSpinner(Instantiate(columnData[i].spinner[j]));
                }
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
        for (int i = 0; i < columnData.Count; i++)
        {
            if (columnData[i].column)
            {
                float delay = Random.Range(eachColumnSpinStartDelayDurationMin, eachColumnSpinStartDelayDurationMax);
                columnData[i].column.StartSpinning();

                yield return new WaitForSeconds(delay);
            }
        }
    }

}

[System.Serializable]
public class ColumnSpinnerHelper
{
    public SlotColumn column;
    public List<SpinnerSO> spinner;
}
