using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.GameData
{
    [Serializable]
    public static class GameDataManager
    {
        private const string playerFile = "player.bin";
        static void test()
        {
            var a = LoadData<LevelData>(1);
        }

        public static T LoadData<T>(int index)
        {
            var filename = GetFileNameByType<T>(index);
            string path = Path.Combine(Application.persistentDataPath, filename);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                T data = (T)formatter.Deserialize(stream);
                stream.Close();

                return data;
            }
            else
            {
                Debug.Log($"File: {path} not found!");
                throw new IOException($"file not found: {path}");
            }
        }

        public static void SaveLevelData(LevelData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, $"level{data.levelNumber}.bin");
            
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        private static string GetFileNameByType<T>(int index)
        {
            if (typeof(T) == typeof(LevelData))
            {
                return $"level{index}.bin";
            }

            return string.Empty;
        }

        public static void SavePlayerData(PlayerData playerData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, playerFile);

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, playerData);
            stream.Close();
        }

        public static PlayerData LoadPlayerData()
        {
            string path = Path.Combine(Application.persistentDataPath, playerFile);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                PlayerData data = (PlayerData)formatter.Deserialize(stream);
                stream.Close();

                if (data == null)
                {
                    data = new PlayerData();
                }

                PlayerData.Instance = data;
                return data;
            }
            else
            {
                Debug.Log($"File: {path} not found! New Instance Created");
                PlayerData.Instance = new PlayerData();
                return PlayerData.Instance;
            }
        }
    }
}