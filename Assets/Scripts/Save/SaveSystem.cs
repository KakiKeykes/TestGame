using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Save
{
    public class SaveSystem
    {
        private static string SavePath => Application.persistentDataPath + "/savefile.json";

        public static void SaveGame(List<ISaveable> saveableObjects)
        {
            SaveData data = new SaveData();

            foreach (var saveable in saveableObjects)
            {
                saveable.SaveData(data);
            }

            string json = data.ToJson();
            File.WriteAllText(SavePath, json);

            Debug.Log("Game saved to " + SavePath);
        }

        public static void LoadGame(List<ISaveable> saveableObjects)
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                SaveData data = SaveData.FromJson(json);

                foreach (var saveable in saveableObjects)
                {
                    saveable.LoadData(data);
                }

                Debug.Log("Game loaded from " + SavePath);
            }
            else
            {
                Debug.LogError("Save file not found in " + SavePath);
            }
        }
    }
}