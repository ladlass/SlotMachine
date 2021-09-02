using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SlotSymbolData
{
    public RectTransform rect;
    public SlotSymbolGameObj slotGameObj;
    public SlotSymbolTypes symbolType;
}
public class SlotColumn : MonoBehaviour
{
    [SerializeField]private float lowerTrasholdPos = -400;
    private float imageVerticalSize;

    private List<SlotSymbolData> slotSymbols = new List<SlotSymbolData>();
    private SlotSymbolPool symbolPool;
    private SpinHandler    spinHandler;
    private SpinnerManager spinnerManager;

    private float yPositionOfTopSymbol;
    private SlotSymbolTypes selectedSymbol;
    private bool areSymbolsBlurry = false;

    public bool IsSpinning { get; private set; } = false;

    public void AwakeSpinner(float imageVerticalSize)
    {
        this.imageVerticalSize = imageVerticalSize;

        ResetPositionOfTopSymbol();
        for (int i = 0; i < 3; i++)
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
        yPositionOfTopSymbol = imageVerticalSize *(slotSymbols.Count - 2);//2 for bottom and mid image
    }

    public void StartSpinning(SlotSymbolTypes selectedSymbol, List<SlotSymbolTypes> selectedSymbols)
    {
        if (IsSpinning == true) return;

        this.selectedSymbol = selectedSymbol;
        ResetPositionOfTopSymbol();

        spinHandler.StartSpinning(selectedSymbols);
        IsSpinning = true;
    }

    private void CreateSelectedSymbol()
    {
        SlotSymbolData slotData = null;
        slotData = symbolPool.AcquireSymbol(selectedSymbol, transform);

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
        SlotSymbolData slotData = null;
        int enumCount = Enum.GetNames(typeof(SlotSymbolTypes)).Length;
        int randomIndex = UnityEngine.Random.Range(0, enumCount);
        slotData = symbolPool.AcquireSymbol((SlotSymbolTypes)randomIndex, transform);

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
        if (IsSpinning == false && spinHandler == null) return;

        IsSpinning = spinHandler.UpdateSpinner(deltaTime);
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
