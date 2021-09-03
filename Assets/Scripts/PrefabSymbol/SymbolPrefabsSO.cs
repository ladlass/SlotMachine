using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine
{
    [CreateAssetMenu(fileName = "Symbol", menuName = "ScriptableObjects/Symbols", order = 1)]
    public class SymbolPrefabsSO : ScriptableObject
    {
        public List<SymbolPrefabData> prefabs;
        public SlotSymbolGameObj prefabClass;
        public Dictionary<SlotSymbolTypes, SymbolPrefabData> CreateDictionary()
        {
            Dictionary<SlotSymbolTypes, SymbolPrefabData> symbolPrefabs = new Dictionary<SlotSymbolTypes, SymbolPrefabData>();
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i] != null)
                {

                    symbolPrefabs.Add(prefabs[i].type, prefabs[i]);
                }
            }
            return symbolPrefabs;
        }
    }
}