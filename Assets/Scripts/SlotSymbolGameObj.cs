using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class SlotSymbolGameObj : MonoBehaviour
    {
        private Image imageComp;
        private SymbolPrefabData data;
        void Awake()
        {
            imageComp = GetComponent<Image>();
        }

        public void SetPrefabData(SymbolPrefabData data)
        {
            this.data = data;

            SetBlur(false);
        }

        public void SetBlur(bool isBlurry)
        {
            if (data != null)
            {
                if (isBlurry)
                {
                    imageComp.sprite = data.blurryImage;
                }
                else
                {
                    imageComp.sprite = data.normalImage;
                }
            }
        }
    }
}