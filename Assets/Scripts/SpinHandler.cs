using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [Serializable]
    public class SpinHandler
    {
        private List<SlotSymbolData> slotSymbols;
        private SpinnerManager spinnerManager;

        private int symbolCounterToStop = 0;
        private float lowerThresholdPos = -400;

        private Action<int, bool> onSymbolReachesThreshold;
        private Action<bool> onBlurSet;

        public bool IsSpinning { get; private set; } = false;

        public SpinHandler(SpinnerManager spinnerManager, List<SlotSymbolData> slotSymbols, Action<int, bool> onSymbolReachesThreshold, Action<bool> onBlurSet, float lowerThresholdPos)
        {
            this.slotSymbols = slotSymbols;
            this.spinnerManager = spinnerManager;
            this.lowerThresholdPos = lowerThresholdPos;

            this.onSymbolReachesThreshold = onSymbolReachesThreshold;
            this.onBlurSet = onBlurSet;

        }

        public void StartSpinning(List<SlotSymbolTypes> selectedSymbols)
        {
            if (IsSpinning == true) return;

            IsSpinning = true;
            spinnerManager.Reset(selectedSymbols);
            symbolCounterToStop = spinnerManager.GetScrollCountToStop();
            symbolCounterToStop -= Mathf.Abs(slotSymbols.Count - 2);//-2 for bottom + mid symbol 

            onBlurSet?.Invoke(true);
        }
        public bool UpdateSpinner(float deltaTime)
        {
            if (IsSpinning == false) return false;

            float posOffsetY = spinnerManager.Spin(deltaTime);

            if (spinnerManager.IsSpinningEnded())
            {
                IsSpinning = false;
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
                        if (symbolCounterToStop == 0)
                        {
                            onBlurSet?.Invoke(false);
                        }
                    }
                }
            }
        }

    }
}