using System;
using System.IO;
using System.Text;
using UnityEngine;
using Game.Core.DTOs;

namespace Game.Infrastructure.Persistence
{
    /// JSON-based implementation of save repository. (I can add encryption for safety in future and async Save/Load)
    public class JsonSaveSystem : IRepo<SaveData>
    {
        private readonly string _filePath;

        public JsonSaveSystem(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Save file name cannot be empty.", nameof(fileName));

            _filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        /// Save data to JSON file on disk.
        public void Save(SaveData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(_filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveSystem] Failed to save data: {ex}");
            }
        }

        /// Load data from disk.
        public SaveData Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new SaveData();

                string json = File.ReadAllText(_filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                    return new SaveData();

                var data = JsonUtility.FromJson<SaveData>(json);
                return data ?? new SaveData();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[JsonSaveSystem] Failed to load save: {ex}. Creating new save.");
                return new SaveData();
            }
        }

        /// Delete save file from disk.
        public void Delete()
        {
            try
            {
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[JsonSaveSystem] Failed to delete save file: {ex}");
            }
        }
    }
}
