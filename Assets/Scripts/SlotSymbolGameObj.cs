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
        [SerializeField]private Image imageComp;
        private SymbolPrefabData data;

        public void SetPrefabData(SymbolPrefabData data)
        {
            this.data = data;

            SetBlur(false);
        }

        public void SetBlur(bool isBlurry)
        {
            if (data != null && imageComp != null)
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