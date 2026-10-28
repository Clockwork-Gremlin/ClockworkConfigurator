using System;

namespace ClockworkConfigurator.Core
{
    public static class SettingsBuilder
    {
        
        public static Setting<T> Register<T>(
            string groupId, 
            string id, 
            string description, 
            T defaultValue, 
            SettingFlags flags = SettingFlags.NONE) where T : IConvertible
        {
            var setting = new Setting<T>(groupId, id, description, defaultValue, flags);
            
            Configurator.RegisterSetting(groupId, setting);
      
            return setting;
        }
        
        public static ClampedSetting<T> RegisterClamped<T>(
            string groupId, 
            string id, 
            string description, 
            T defaultValue, 
            T minimumValue,
            T maximumValue,
            SettingFlags flags = SettingFlags.NONE) where T : IComparable, IConvertible
        {
            var setting = new ClampedSetting<T>(groupId, id, description, defaultValue, minimumValue, maximumValue, flags);
            Configurator.RegisterSetting(groupId, setting);
            return setting;
        }
        
        public static ClampedSetting<int> RegisterPercentage(
            string groupId, 
            string id, 
            string description, 
            int defaultValue,
            SettingFlags flags = SettingFlags.NONE)
        {
            var setting = new ClampedSetting<int>(groupId, id, description, defaultValue, 0, 100, flags);
            Configurator.RegisterSetting(groupId, setting);
            return setting;
        }
        
    }
}