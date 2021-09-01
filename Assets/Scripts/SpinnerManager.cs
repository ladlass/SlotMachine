using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerManager 
{
    private List<SpinnerSO> spinners;
    private int index = 0;
    private int scrollCount = 0;
    private float imageVerticalSize = 0;
    public SpinnerManager(float imageVerticalSize)
    {
        spinners = new List<SpinnerSO>();
        this.imageVerticalSize = imageVerticalSize;
    }
    public void AddSpinner(SpinnerSO spinner)
    {
        spinners.Add(spinner);
    }

    public void AddSpinner(List<SpinnerSO> spinner)
    {
        spinners.AddRange(spinner);
    }

    public void Reset()
    {
        index = 0;
        scrollCount = 0;
        for (int i = 0; i < spinners.Count; i++)
        {
            spinners[i].ResetPositionHandler(imageVerticalSize);
            scrollCount += spinners[i].GetScrollCount();
        }

    }

    public int GetScrollCountToStop()
    {
        return scrollCount;
    }
    public float Spin(float deltaTime)
    {
        if (IsSpinningEnded()) return 0;

        float offset = spinners[index].UpdatePosition(deltaTime);

        if (spinners[index].IsTimeUp())
        {
            index++;
        }

        return offset;
    }


    public bool IsSpinningEnded()
    {
        return index >= spinners.Count;
    }

}
