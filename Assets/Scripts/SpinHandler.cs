using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinHandler
{
    private float lowerThresholdPos = -400;
    private List<SlotSymbolData> slotSymbols;
    private Action<int, bool > onSymbolReachesThreshold;
    public bool IsSpinning { get; private set; } = false;
    private SpinnerManager spinnerManager;
    private int symbolCounterToStop = 0;

    public SpinHandler(SpinnerManager spinnerManager, List<SlotSymbolData> slotSymbols, Action<int, bool> onSymbolReachesThreshold, float lowerThresholdPos)
    {
        this.spinnerManager = spinnerManager;
        this.slotSymbols = slotSymbols;
        this.onSymbolReachesThreshold = onSymbolReachesThreshold;
        this.lowerThresholdPos = lowerThresholdPos;
    }

    public void StartSpinning()
    {
        if (IsSpinning == true) return;

        IsSpinning = true;
        spinnerManager.Reset();
        symbolCounterToStop = spinnerManager.GetScrollCountToStop();
        symbolCounterToStop -= Mathf.Abs(slotSymbols.Count - 2);//-2 for bottom + mid symbol 
    }
    public bool UpdateSpinner(float deltaTime)
    {
        if (IsSpinning == false) return false;

        float posOffsetY = spinnerManager.Spin(deltaTime);

        if (spinnerManager.IsSpinningEnded())
        {
            IsSpinning = false;
            //OnSpinStop
        }

        SpinColumn(posOffsetY);

        return IsSpinning;
    }

    private void SpinColumn(float movementAmount)
    {
        Vector2 newPos = Vector2.zero;
        for (int i = slotSymbols.Count - 1; i >= 0; i--)
        {
            SlotSymbolData itemData = slotSymbols[i];
            RectTransform rect = itemData.rect;
            if (itemData != null && rect)
            {
                newPos.y = rect.anchoredPosition.y + movementAmount;
                rect.anchoredPosition = newPos;

                if (newPos.y < lowerThresholdPos)
                {
                    symbolCounterToStop--;

                    onSymbolReachesThreshold?.Invoke(i, symbolCounterToStop == 0);
                }
            }
        }
    }
  
}
