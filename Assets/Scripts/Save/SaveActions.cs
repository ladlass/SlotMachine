using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SlotMachine
{
    public class SaveActions
    {
        private string fileName = "SaveFile.json";

        public void SaveGame(SaveData save)
        {
            Save(save);
        }
        public SaveData LoadGame()
        {
            SaveData save = Load();
            if (save != null)
            {
                return save;
            }
            return null;
        }
        private void Save(SaveData save)
        {
            string json = JsonUtility.ToJson(save);
            Debug.Log(json);
            WriteToFile(json, fileName);
        }

        private SaveData Load()
        {
            SaveData loadData = new SaveData();
            string json = ReadFromFile(fileName);
            if (json != null)
            {
                JsonUtility.FromJsonOverwrite(json, loadData);
                return loadData;
            }
            else
            {
                return null;
            }
        }
        public void DeleteFile(int profileNum)
        {
            string filePath = GetFilePath(fileName);

            // check if file exists
            if (!File.Exists(filePath))
            {
                //print("File to be deleted is not exist");
            }
            else
            {
                File.Delete(filePath);
            }
        }
        public string GetFilePath(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }
        string ReadFromFile(string fileName)
        {
            string path = GetFilePath(fileName);
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string json = reader.ReadToEnd();
                reader.Close();
                return json;
            }
            else
            {
                //print("No Path Exist");
                return null;
            }

        }
        void WriteToFile(string json, string fileName)
        {
            FileStream fileStream = new FileStream(GetFilePath(fileName), FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(json);
            writer.Close();
        }
    }
}