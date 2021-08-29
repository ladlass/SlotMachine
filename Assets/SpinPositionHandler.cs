using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPositionHandler
{
    //[SerializeField] private float lowerTrasholdPos = -400;
    //private float speedMultiplier = 5;
    //private float speed => imageVerticalSize * speedMultiplier;

    //private float duration = 4;
    //private float imageVerticalSize = 260;

    //private float yPositionOfTopSymbol;

    //private int symbolCountToScroll;
    //private int symbolCounterToFinish;

    //private bool enableSpin = false;

    //private float timer = 0;
    //private float spinCurrentPositionY;

    //private void SetSymbolTopPositionY(int currentSymbolCount)
    //{
    //    yPositionOfTopSymbol = imageVerticalSize * (currentSymbolCount - 2);
    //}
   

    //public void StartSpinning(float duration, float speedMultiplier, int currentSymbolCount)
    //{
    //    SetSymbolTopPositionY(currentSymbolCount);
    //    timer = 0;
    //    this.duration = duration;
    //    this.speedMultiplier = speedMultiplier;

    //    symbolCountToScroll = Mathf.FloorToInt((duration * speed) / imageVerticalSize);
    //    symbolCounterToFinish = symbolCountToScroll - Mathf.Abs(currentSymbolCount - 2) - 1;//-2 for bottom + mid symbol plus  -1 for the last symbol that will come after the selected

    //    enableSpin = true;
    //}

    //private void OnSymbolAddedTop(float topPosY)
    //{
    //    yPositionOfTopSymbol = topPosY;
    //    yPositionOfTopSymbol += imageVerticalSize;
    //    //rect.anchoredPosition = new Vector3(0, yPositionOfTopSymbol);
    //}

    //public void UpdateSpinner(float deltaTime)
    //{
    //    if (enableSpin == false) return;

    //    timer += deltaTime;

    //    float prevCyclePosY = spinCurrentPositionY;
    //    spinCurrentPositionY = Mathf.Lerp(0, -symbolCountToScroll * imageVerticalSize, timer / duration);

    //    float addedPosition = spinCurrentPositionY - prevCyclePosY;

    //    if (addedPosition == 0.0f) enableSpin = false;

    //    Vector2 newPos = Vector2.zero;
    //    for (int i = slotSymbols.Count - 1; i >= 0; i--)
    //    {
    //        SlotSymbolData itemData = slotSymbols[i];
    //        RectTransform rect = itemData.rect;
    //        if (itemData != null && rect)
    //        {
    //            newPos.y = rect.anchoredPosition.y + addedPosition;
    //            rect.anchoredPosition = newPos;

    //            if (newPos.y < lowerTrasholdPos)
    //            {
    //                slotSymbols.RemoveAt(i);
    //                symbolPool.Add(itemData);
    //                CreateSymbol();
    //                symbolCounterToFinish--;
    //            }
    //        }
    //    }
    //}
}
