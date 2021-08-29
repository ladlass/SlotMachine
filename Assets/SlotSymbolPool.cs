using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotSymbolPool
{
    [SerializeField]private List<SlotSymbolData> slotItemsPool;
    private Dictionary<SlotSymbolTypes, GameObject> symbolPrefabs;

    public SlotSymbolPool(Dictionary<SlotSymbolTypes, GameObject> symbolPrefabs)
    {
        slotItemsPool = new List<SlotSymbolData>();
        this.symbolPrefabs = symbolPrefabs;
    }

    public void Add(SlotSymbolData newData)
    {
        if(slotItemsPool != null)
        {
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
        else if(symbolToReturn.rect)
        {
            symbolToReturn.rect.transform.SetParent(parent);
        }
        return symbolToReturn;
    }

    private SlotSymbolData CreateSymbol(SlotSymbolTypes type, Transform parent)
    {

        GameObject clone = GameObject.Instantiate(GetPrefabByType(type), parent);

        SlotSymbolData newSymbolData = new SlotSymbolData();
        newSymbolData.rect = clone.GetComponent<RectTransform>();

        return newSymbolData;
    }

    private GameObject GetPrefabByType(SlotSymbolTypes type)
    {
        if (symbolPrefabs.ContainsKey(type))
        {
            return symbolPrefabs[type];
        }

        return null;
    }

    private SlotSymbolData PopSymbolFromPool(SlotSymbolTypes type)
    {
        if (slotItemsPool == null) return null;

        SlotSymbolData symbolToReturn = null;

        for (int i = 0; i < slotItemsPool.Count; i++)
        {
            if(slotItemsPool[i] != null && slotItemsPool[i].symbolType == type)
            {
                symbolToReturn = slotItemsPool[i];
                slotItemsPool.RemoveAt(i);
                break;
            }
        }

        return symbolToReturn;
    }

    //public SlotSymbolData PopIfExist(SlotSymbolTypes type)
    //{
    //    if (slotItemsPool != null && )
    //    {
    //        slotItemsPool.Add(newData);
    //    }
    //}

    //private void CreateItem(bool enableSpawningSelected)
    //{

    //}

}
