using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Symbol", menuName = "ScriptableObjects/Symbols", order = 1)]
public class SymbolPrefabsSO : ScriptableObject
{
    public List<SymbolPrefabData> prefabs;

    public Dictionary<SlotSymbolTypes, GameObject> CreateDictionary()
    {
        Dictionary<SlotSymbolTypes, GameObject> symbolPrefabs = new Dictionary<SlotSymbolTypes, GameObject>();
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i] != null)
            {
                symbolPrefabs.Add(prefabs[i].type, prefabs[i].prefab);
            }
        }
        return symbolPrefabs;
    }
}

[System.Serializable]
public class SymbolPrefabData
{
    public GameObject prefab;
    public SlotSymbolTypes type;
}
