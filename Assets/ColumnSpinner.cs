using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotSymbolTypes
{
    A, Seven, Bonus, Jackpot
}
[System.Serializable]
public class SlotSymbolData
{
    public RectTransform rect;
    public SlotSymbolTypes symbolType;
}
public class ColumnSpinner : MonoBehaviour
{
    [SerializeField]private float lowerTrasholdPos = -400;
    [SerializeField]private float imageVerticalSize = 260;

    [SerializeField] private List<SlotSymbolData> slotSymbols = new List<SlotSymbolData>();
    private SlotSymbolPool symbolPool;
    private RectTransform columnSpinnerRect;

    private float speedMultiplier = 5;
    private float speed => imageVerticalSize * speedMultiplier;

    private float duration = 4;

    private int symbolCountToScroll;
    private int symbolCounterToStop;

    private float timer = 0;
    private float yPositionOfTopItem;
    private float spinCurrentPositionY;
    
    private bool enableSpin = false;

    public void AwakeSpinner()
    {
        columnSpinnerRect = GetComponent<RectTransform>();
        yPositionOfTopItem = imageVerticalSize * (slotSymbols.Count - 2);
        
        for (int i = 0; i < 3; i++)
        {
            CreateSymbol();
        }
    }

    public void SetPool(SlotSymbolPool symbolPool)
    {
        this.symbolPool = symbolPool;
    }

    public void StartSpinning(float duration, float speedMultiplier)
    {
        yPositionOfTopItem = imageVerticalSize * (slotSymbols.Count - 2);

        spinCurrentPositionY = 0;
        timer = 0;
        this.duration = duration;
        this.speedMultiplier = speedMultiplier;

        symbolCountToScroll = Mathf.FloorToInt((duration * speed) / imageVerticalSize);
        symbolCounterToStop = symbolCountToScroll - Mathf.Abs(slotSymbols.Count - 2) - 1;//-2 for bottom + mid symbol plus  -1 for the last symbol that will come after the selected

        enableSpin = true;
    }
    
    private void CreateSymbol()
    {
        SlotSymbolData slotData = null;
        if (enableSpin && symbolCounterToStop == 0)
        {
            slotData = symbolPool.AcquireSymbol(SlotSymbolTypes.Seven, transform);
        }
        else
        {
            int enumCount = Enum.GetNames(typeof(SlotSymbolTypes)).Length;
            int randomIndex = UnityEngine.Random.Range(0, enumCount);
            slotData = symbolPool.AcquireSymbol((SlotSymbolTypes)randomIndex, transform);
        }

        RectTransform rect = slotData.rect;

        if (slotSymbols.Count - 1 >= 0 && slotSymbols[slotSymbols.Count - 1].rect)
        {
            yPositionOfTopItem = slotSymbols[slotSymbols.Count - 1].rect.anchoredPosition.y;
        }
        yPositionOfTopItem += imageVerticalSize;


        rect.anchoredPosition = new Vector3(0, yPositionOfTopItem);

        slotSymbols.Add(slotData);
    }

    public void UpdateSpinner(float deltaTime)
    {
        if (enableSpin == false) return;

        timer += deltaTime;

        float prevCyclePosY = spinCurrentPositionY;
        spinCurrentPositionY = Mathf.Lerp(0, -symbolCountToScroll * imageVerticalSize, timer / duration);

        float posOffsetY = spinCurrentPositionY - prevCyclePosY;

        if (posOffsetY == 0.0f)
        {
            enableSpin = false;
            return;
        }

        SpinColumn(posOffsetY);
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

                if (newPos.y < lowerTrasholdPos)
                {
                    OnSymbolOutOfBounds(i);
                }
            }
        }
    }
    private void OnSymbolOutOfBounds(int index)
    {
        SlotSymbolData itemData = slotSymbols[index];
        slotSymbols.RemoveAt(index);
        symbolPool.Add(itemData);
        CreateSymbol();
        symbolCounterToStop--;
    }
}
