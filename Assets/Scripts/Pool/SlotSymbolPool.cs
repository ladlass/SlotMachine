using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    [System.Serializable]
    public class SlotSymbolPool
    {
        [SerializeField] private List<SlotSymbolData> slotItemsPool;
        private Dictionary<SlotSymbolTypes, SymbolPrefabData> symbolPrefabsData;
        private SlotSymbolGameObj prefab;
        private Transform garbage;
        public SlotSymbolPool(Dictionary<SlotSymbolTypes, SymbolPrefabData> symbolPrefabsData, Transform garbage, SlotSymbolGameObj prefab)
        {
            slotItemsPool = new List<SlotSymbolData>();
            this.symbolPrefabsData = symbolPrefabsData;
            this.garbage = garbage;
            this.prefab = prefab;
        }

        public void Add(SlotSymbolData newData)
        {
            if (slotItemsPool != null)
            {
                if (newData.rect)
                {
                    newData.rect.transform.SetParent(garbage);
                    newData.rect.gameObject.SetActive(false);
                }
                slotItemsPool.Add(newData);
            }
        }


        public SlotSymbolData AcquireSymbol(SlotSymbolTypes type, Transform parent)
        {
            SlotSymbolData symbolToReturn = PopSymbolFromPool(type);

            if (symbolToReturn == null)
            {
                symbolToReturn = CreateSymbol(type, parent);
            }
            else if (symbolToReturn.rect)
            {
                symbolToReturn.rect.transform.SetParent(parent);
            }
            return symbolToReturn;
        }

        private SlotSymbolData CreateSymbol(SlotSymbolTypes type, Transform parent)
        {
            if (prefab == null) return null;

            GameObject clone = Object.Instantiate(prefab.gameObject, parent);
            SlotSymbolGameObj newSymbol = clone.GetComponent<SlotSymbolGameObj>();

            if (newSymbol == null) return null;

            newSymbol.SetPrefabData(GetPrefabDataByType(type));
            SlotSymbolData newSymbolData = new SlotSymbolData();
            newSymbolData.rect = clone.GetComponent<RectTransform>();
            newSymbolData.slotGameObj = newSymbol;
            newSymbolData.symbolType = type;

            return newSymbolData;
        }

        private SymbolPrefabData GetPrefabDataByType(SlotSymbolTypes type)
        {
            if (symbolPrefabsData.ContainsKey(type))
            {
                return symbolPrefabsData[type];
            }

            return null;
        }

        private SlotSymbolData PopSymbolFromPool(SlotSymbolTypes type)
        {
            if (slotItemsPool == null) return null;

            SlotSymbolData symbolToReturn = null;

            for (int i = 0; i < slotItemsPool.Count; i++)
            {
                if (slotItemsPool[i] != null && slotItemsPool[i].symbolType == type)
                {
                    symbolToReturn = slotItemsPool[i];
                    slotItemsPool.RemoveAt(i);
                    break;
                }
            }

            if (symbolToReturn != null)
            {
                if (symbolToReturn.rect)
                {
                    symbolToReturn.rect.gameObject.SetActive(true);
                }
                if (symbolToReturn.slotGameObj)
                {
                    symbolToReturn.slotGameObj.SetPrefabData(GetPrefabDataByType(type));
                }
            }

            return symbolToReturn;
        }
    }
}