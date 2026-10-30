using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ClockworkConfigurator.Core
{
    public static class Configurator
    {
        public static ISettingsManager SettingsManager {get; set;}

        public static Dictionary<string, SettingBase> RegisteredSettings { get; private set; } = new();
        private static Dictionary<string, object> loadedSettings = new();
        
        public static void Log(string message) => SettingsManager?.Log(message);
        public static void LogWarning(string message) => SettingsManager?.LogWarning(message);
        public static void LogError(string message) => SettingsManager?.LogError(message);

        public static Action<SettingBase> SettingRegistered;
        
        internal static void RegisterSetting(string settingsGroup, SettingBase setting)
        {
            
            if (RegisteredSettings.ContainsKey(setting.Id))
                RegisteredSettings.Remove(setting.Id);
            RegisteredSettings.Add(setting.Id, setting);

            if(setting.HasFlag(SettingFlags.SERIALIZED) && loadedSettings.ContainsKey(setting.Id))
            {
                setting.ParseUnsafe(loadedSettings[setting.Id]);
            }
        }
        
        public static void ResetSettingsGroup(string settingsGroup)
        {
            foreach (var setting in RegisteredSettings.Values)
            {
                if(settingsGroup == setting.GroupId) 
                    setting.Reset();
            }
        }
        

        public static void Save()
        {
            var filePath = Path.Combine(SettingsManager.GetFilePath(), SettingsManager.FileName);
   
            Log($"Attempting to save config info to {filePath}");
            
            var saveData = new Dictionary<string, Dictionary<string, object>>();
            
            foreach (var setting in RegisteredSettings)
            {
                if (setting.Value.HasFlag(SettingFlags.SERIALIZED))
                {
                    if(!saveData.ContainsKey(setting.Value.GroupId))
                        saveData.Add(setting.Value.GroupId, new Dictionary<string, object>());
                    saveData[setting.Value.GroupId].Add(setting.Key, setting.Value.GetWrappedValue());
                }
            }
            
            var saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            
            if (!Directory.Exists(SettingsManager.GetFilePath()))
                Directory.CreateDirectory(SettingsManager.GetFilePath());
            File.WriteAllText(filePath, saveJson);
        }
        
        public static void Load()
        {
            var filePath = Path.Combine(SettingsManager.GetFilePath(), SettingsManager.FileName);
            Log(filePath);
            
            if (!File.Exists(filePath))
            {
                Log("Failed to find config file, falling back to defaults...");
                return;
            }
            
            var json = File.ReadAllText(filePath);
            var loadData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
            loadedSettings = new();

            if (loadData == null) return;

            foreach (var loadSettingGroup in loadData)
            {
                foreach (var loadingSetting in loadSettingGroup.Value)
                {
                    loadedSettings.Add(loadingSetting.Key, loadingSetting.Value);
                }
            }
        }

    }
}
