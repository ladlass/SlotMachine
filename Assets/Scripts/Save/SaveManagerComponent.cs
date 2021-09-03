using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SaveManagerComponent : MonoBehaviour
    {
        private SaveActions saveActions;

        public void AwakeComponent()
        {
            saveActions = new SaveActions();
        }

        public SaveData LoadGame()
        {
            if (saveActions == null) return null;

            return saveActions.LoadGame();
        }

        public void SaveGame(SlotRandomManagerComponent randComp)
        {
            if (saveActions == null) return;

            SaveData newSave = new SaveData();
            newSave.currentRandomSeqIndex = randComp.GetCurrentIndex();
            newSave.randomSeqData = randComp.GetRandSeqData();

            saveActions.SaveGame(newSave);
        }
    }
}