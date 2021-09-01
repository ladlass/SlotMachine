using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SlotSymbolData
{
    public RectTransform rect;
    public SlotSymbolTypes symbolType;
}
public class SlotColumn : MonoBehaviour
{
    [SerializeField]private float lowerTrasholdPos = -400;
    private float imageVerticalSize;

    private List<SlotSymbolData> slotSymbols = new List<SlotSymbolData>();
    private SpinHandler    spinHandler;
    private SlotSymbolPool symbolPool;
    private SpinnerManager spinnerManager;

    private float yPositionOfTopSymbol;
    public bool IsSpinning { get; private set; } = false;

    public void AwakeSpinner(float imageVerticalSize)
    {
        this.imageVerticalSize = imageVerticalSize;

        ResetPositionOfTopSymbol();

        for (int i = 0; i < 3; i++)
        {
            CreateRandomSymbol();
        }

        spinHandler = new SpinHandler(spinnerManager, slotSymbols, OnSymbolOutOfBounds, lowerTrasholdPos);
    }

    public void SetPool(SlotSymbolPool symbolPool)
    {
        this.symbolPool = symbolPool;
    }

    public void SetSpinnerManager(SpinnerManager spinnerManager)
    {
        this.spinnerManager = spinnerManager;
    }

    private void ResetPositionOfTopSymbol()
    {
        yPositionOfTopSymbol = imageVerticalSize *(slotSymbols.Count - 2);
    }

    public void StartSpinning()
    {
        if (IsSpinning == true) return;

        ResetPositionOfTopSymbol();

        spinHandler.StartSpinning();
        IsSpinning = true;
    }

    private void CreateSelectedSymbol()
    {
        SlotSymbolData slotData = null;
        slotData = symbolPool.AcquireSymbol(SlotSymbolTypes.Seven, transform);

        SetSymbolPositionToTop(slotData);
        slotSymbols.Add(slotData);
    }

    private void CreateRandomSymbol()
    {
        SlotSymbolData slotData = null;
        int enumCount = Enum.GetNames(typeof(SlotSymbolTypes)).Length;
        int randomIndex = UnityEngine.Random.Range(0, enumCount);
        slotData = symbolPool.AcquireSymbol((SlotSymbolTypes)randomIndex, transform);

        SetSymbolPositionToTop(slotData);
        slotSymbols.Add(slotData);
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
