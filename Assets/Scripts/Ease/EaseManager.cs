using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
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
            switch (type)
            {
                case EaseTypes.Lineer:
                    return Mathf.Lerp(start, end, value);
                case EaseTypes.EaseOutCubic:
                    return EaseOutCubic(start, end, value);
                case EaseTypes.EaseInOutQuad:
                    return EaseInOutQuad(start, end, value);
                case EaseTypes.EaseOutSine:
                    return EaseOutSine(start, end, value);
                default:
                    return Mathf.Lerp(start, end, value);
            }
        }
    }
}