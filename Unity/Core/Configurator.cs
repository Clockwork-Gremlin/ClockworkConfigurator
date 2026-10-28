using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ClockworkConfigurator.Core
{
    public static class Configurator
    {
        public static Dictionary<string, SettingBase> RegisteredSettings { get; private set; } = new();
        
        public static Action<String> LogWarning;
        public static Action<String> LogError;
        public static Action<String> Log;

        public static Action<SettingBase> SettingRegistered;
        
        internal static void RegisterSetting(string settingsGroup, SettingBase settingBase)
        {
            if (RegisteredSettings.ContainsKey(settingBase.ID()))
                RegisteredSettings.Remove(settingBase.ID()); //clean up any lingering static data from previous runs
            RegisteredSettings.Add(settingBase.ID(), settingBase);
        }
        
        public static void OnStart()
        {
            LogWarning = null;
            LogError = null;
            Log = null;
            SettingRegistered = null;
        }

        public static void OnQuit()
        {
            
        }
        
        public static void ResetSettingsGroup(string settingsGroup)
        {
            foreach (var setting in RegisteredSettings.Values)
            {
                if(settingsGroup == setting.GroupId()) 
                    setting.Reset();
            }
        }
        

        public static void Save(string directory, string filename)
        {
            var filePath = Path.Combine(directory, filename);
   
            Log?.Invoke($"Attempting to save config info to {filePath}");
            
            var saveData = new Dictionary<string, Dictionary<string, object>>();
            
            foreach (var setting in RegisteredSettings)
            {
                if (setting.Value.HasFlag(SettingFlags.SERIALIZED))
                {
                    if(!saveData.ContainsKey(setting.Value.GroupId()))
                        saveData.Add(setting.Value.GroupId(), new Dictionary<string, object>());
                    saveData[setting.Value.GroupId()].Add(setting.Key, setting.Value.GetWrappedValue());
                }
            }
            
            var saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllText(filePath, saveJson);
        }
        
        public static void Load(string directory, string filename)
        {
            var filePath = Path.Combine(directory, filename);

            Log?.Invoke($"Attempting to pull saved config info from {filePath}");
            
            if (!File.Exists(filePath))
            {
                Log?.Invoke("Failed to find config file, falling back to defaults...");
                return;
            }
            
            var json = File.ReadAllText(filePath);
            var loadData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

            if (loadData == null) return;

            foreach (var loadSettingGroup in loadData)
            {
                foreach (var loadingSetting in loadSettingGroup.Value)
                {
                    if (RegisteredSettings.ContainsKey(loadingSetting.Key))
                    {
                        var registeredSetting = RegisteredSettings[loadingSetting.Key];
                        if (registeredSetting.HasFlag(SettingFlags.SERIALIZED))
                        {
                            registeredSetting.ParseUnsafe(loadingSetting.Value);
                        }
                        else
                        {
                            LogWarning?.Invoke($"Attempted to load non-serialized setting {loadingSetting.Key}, discarding...");
                        }
                    }
                    else
                    {
                        LogWarning?.Invoke($"Attempted to load unknown setting with name {loadingSetting.Key}, ignoring...");
                    }
                }
            }
        }

    }
}