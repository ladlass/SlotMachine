using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SlotMachine
{
    public class SlotColumn : MonoBehaviour
    {
        [SerializeField] private float lowerTrasholdPos = -400;
        private float imageVerticalSize;

        private List<SlotSymbolData> slotSymbols = new List<SlotSymbolData>();
        private SpinHandler spinHandler;
        private SlotSymbolPool symbolPool;
        private SpinnerManager spinnerManager;

        private float yPositionOfTopSymbol;
        private SlotSymbolTypes selectedSymbol;
        private bool areSymbolsBlurry = false;

        private Action onColumnStop;

        [FormerlySerializedAs("halfColumnSymbolCapacity")] public int halfColumnSymbolCapacityWithoutCenter; 
        public bool IsSpinning { get; private set; } = false;

        public void AwakeSpinner(float imageVerticalSize)
        {
            if(imageVerticalSize <= 0) return;
            
            this.imageVerticalSize = imageVerticalSize;
            
            halfColumnSymbolCapacityWithoutCenter = (int)(Mathf.Abs(lowerTrasholdPos) - (imageVerticalSize / 2));//remove half the size of center symbol
            halfColumnSymbolCapacityWithoutCenter = Mathf.CeilToInt(halfColumnSymbolCapacityWithoutCenter / imageVerticalSize);
            
            ResetPositionOfTopSymbol();
            for (int i = 0; i < 2 * halfColumnSymbolCapacityWithoutCenter + 1; i++)//2 half + center symbol
            {
                CreateRandomSymbol();
            }

            spinHandler = new SpinHandler(spinnerManager, slotSymbols, OnSymbolOutOfBounds, SetSymbolBlur, lowerTrasholdPos);
        }

        public void SetPool(SlotSymbolPool symbolPool)
        {
            this.symbolPool = symbolPool;
        }

        public void SetSpinnerManager(SpinnerManager spinnerManager)
        {
            this.spinnerManager = spinnerManager;
        }

        private void SetSymbolBlur(bool isBlurry)
        {
            areSymbolsBlurry = isBlurry;
            for (int i = 0; i < slotSymbols.Count; i++)
            {
                if (slotSymbols[i] != null && slotSymbols[i].slotGameObj != null)
                {
                    slotSymbols[i].slotGameObj.SetBlur(isBlurry);
                }
            }
        }

        private void ResetPositionOfTopSymbol()
        {
            yPositionOfTopSymbol = imageVerticalSize * (-halfColumnSymbolCapacityWithoutCenter - 1);//2 for bottom and mid image
        }

        public void StartSpinning(SlotSymbolTypes selectedSymbol, List<SlotSymbolTypes> selectedSymbols, Action onColumnStop)
        {
            if (IsSpinning == true) return;

            this.onColumnStop = onColumnStop;
            this.selectedSymbol = selectedSymbol;

            spinHandler.StartSpinning(selectedSymbols);
            IsSpinning = true;
        }
        

        private void CreateSelectedSymbol()
        {
            SlotSymbolData slotData = symbolPool.AcquireSymbol(selectedSymbol, transform);

            if (slotData != null)
            {
                if (slotData.slotGameObj)
                {
                    slotData.slotGameObj.SetBlur(areSymbolsBlurry);
                }
                SetSymbolPositionToTop(slotData);
                slotSymbols.Add(slotData);
            }
        }

        private void CreateRandomSymbol()
        {
            int enumCount = Enum.GetNames(typeof(SlotSymbolTypes)).Length;
            int randomIndex = UnityEngine.Random.Range(0, enumCount);
            SlotSymbolData slotData = symbolPool.AcquireSymbol((SlotSymbolTypes)randomIndex, transform);

            if (slotData != null)
            {
                if (slotData.slotGameObj)
                {
                    slotData.slotGameObj.SetBlur(areSymbolsBlurry);
                }

                SetSymbolPositionToTop(slotData);
                slotSymbols.Add(slotData);
            }
        }
        private void SetSymbolPositionToTop(SlotSymbolData slotData)
        {
            if (slotData == null || slotData.rect == null) return;

            RectTransform rect = slotData.rect;

            if (slotSymbols.Count - 1 >= 0 && slotSymbols[slotSymbols.Count - 1].rect)
            {
                yPositionOfTopSymbol = slotSymbols[slotSymbols.Count - 1].rect.anchoredPosition.y;
            }
            yPositionOfTopSymbol += imageVerticalSize;

            rect.anchoredPosition = new Vector3(0, yPositionOfTopSymbol);
        }

        public void UpdateSpinner(float deltaTime)
        {
            if (IsSpinning == false || spinHandler == null) return;

            IsSpinning = spinHandler.UpdateSpinner(deltaTime);

            if (IsSpinning == false)
            {
                onColumnStop?.Invoke();
            }
        }


        private void OnSymbolOutOfBounds(int index, bool spawnSelectedSymbol)
        {
            SlotSymbolData itemData = slotSymbols[index];
            slotSymbols.RemoveAt(index);
            symbolPool.Add(itemData);

            if (spawnSelectedSymbol)
            {
                CreateSelectedSymbol();
            }
            else
            {
                CreateRandomSymbol();
            }
        }
    }
}