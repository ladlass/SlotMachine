using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SlotPoolComponent : MonoBehaviour
    {
        private SlotSymbolPool symbolPool;
        [SerializeField] private SymbolPrefabsSO prefabsList;
        [SerializeField] private Transform garbage;

        public void AwakeComponent()
        {
            symbolPool = new SlotSymbolPool(prefabsList.CreateDictionary(), garbage, prefabsList.prefabClass);
        }

        public SlotSymbolPool GetPool()
        {
            return symbolPool;
        }
    }
}