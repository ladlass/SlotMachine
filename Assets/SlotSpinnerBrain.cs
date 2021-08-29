using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpinnerBrain : MonoBehaviour
{
    [SerializeField]private List<ColumnSpinner> spinners;

    [SerializeField]private SymbolPrefabsSO prefabsList;
    
    [SerializeField]private float speedMultiplier = 5;
    [SerializeField]private float duration = 4;
    [SerializeField] [Range(0, 0.8f)]private float delayedStartDurationMin;
    [SerializeField] [Range(0, 0.8f)]private float delayedStartDurationMax;

    public SlotSymbolPool symbolPool;

    private void Awake()
    {
        symbolPool = new SlotSymbolPool(prefabsList.CreateDictionary());
        PrepareDependencies();
        AwakenSpinners();
    }

    private void PrepareDependencies()
    {
        for (int i = 0; i < spinners.Count; i++)
        {
            if (spinners[i])
            {
                spinners[i].SetPool(symbolPool);
            }
        }
    }

    private void AwakenSpinners()
    {
        for (int i = 0; i < spinners.Count; i++)
        {
            if (spinners[i])
            {
                spinners[i].AwakeSpinner();
            }
        }
    }

    private void UpdateSpinners()
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < spinners.Count; i++)
        {
            if (spinners[i])
            {
                spinners[i].UpdateSpinner(deltaTime);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartPinning();
        }

        UpdateSpinners();
    }

    void StartPinning()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        for (int i = 0; i < spinners.Count; i++)
        {
            if (spinners[i])
            {
                float delay = Random.Range(delayedStartDurationMin, delayedStartDurationMax);
                spinners[i].StartSpinning(duration, speedMultiplier);

                yield return new WaitForSeconds(delay);
            }
        }
    }

}
