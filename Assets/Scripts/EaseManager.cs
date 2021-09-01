using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EaseTypes
{
    Lineer, EaseOutCubic, EaseInOutQuad, EaseOutSine
}
public class EaseManager
{
    public static float EaseOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    }
    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    public static float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }
    public static float GetEaseByType(EaseTypes type, float start, float end, float value)
    {
        if(type == EaseTypes.EaseOutCubic)
        {
            return EaseOutCubic(start, end, value);
        }
        else if(type == EaseTypes.EaseInOutQuad)
        {
            return EaseInOutQuad(start, end,  value);
        }
        else if (type == EaseTypes.EaseOutSine)
        {
            return EaseOutSine(start, end, value);
        }
        else
        {
            return Mathf.Lerp(start, end, value);
        }
    }
}
