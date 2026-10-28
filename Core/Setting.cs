using System;
using System.Collections.Generic;

namespace ClockworkConfigurator.Core
{
    
    public class Setting <T> : SettingBase where T : IConvertible
    {
        public readonly string Name;
        public readonly string DisplayDescription;
        private readonly T _defaultValue;
        private T _temporaryValue;
        private readonly SettingFlags _flags;
        
        private readonly string _groupId;
        public override string GroupId() => _groupId;

        private T _value;
        public T Value => _value;

        public Action<T> OnSettingChanged { get; set; }
        public Action<T> OnSettingApplied { get; set; }

        public Setting(string groupID, string name, string description, T startingValue, SettingFlags flags = SettingFlags.NONE)
        {
            _groupId = groupID;
            _flags = flags;
            _value = startingValue;
            _defaultValue = startingValue;
            Name = name;
            DisplayDescription = description;
        }

        public virtual Setting<T> Set(T newValue, bool asServer = false)
        {
            _temporaryValue = newValue;
            OnSettingChanged?.Invoke(_temporaryValue);
            return this;
        }

        public override void Default()
        {
            _value = _defaultValue;
            _temporaryValue = _defaultValue;
            OnSettingChanged?.Invoke(Value);
            OnSettingApplied?.Invoke(Value);
        }
        
        public override void Reset()
        {
            _temporaryValue = _value;
            OnSettingChanged?.Invoke(Value);
            OnSettingApplied?.Invoke(Value);
        }
        
        public override void Apply(bool send = true)
        {
            _value = _temporaryValue;
            OnSettingApplied?.Invoke(Value);
        }

        public T Get()
        {
            return _value;
        }

        public override void Write(Dictionary<string, dynamic> storageDict)
        {
            if (storageDict.ContainsKey(Name))
                storageDict[Name] = Value;
            else
                storageDict.Add(Name, _value);
        }

        public override void ParseUnsafe(object valueToParse)
        {
            try
            {
                Set((T)Convert.ChangeType(valueToParse, typeof(T)), true).Apply(false);
            }
            catch (Exception e)
            {
                Configurator.LogWarning?.Invoke($"{e.GetType()}! Settings contained definition for {Name} ({valueToParse}), but failed to parse value to corresponding type {typeof(T)}. Reverting to default value.");
            }
        }

        public override string ID()
        {
            return Name;
        }
        
        public override object GetWrappedValue()
        {
            return _value;
        }

        public override bool HasFlag(SettingFlags flags)
        {
            return _flags.HasFlag(flags);
        }
    }
    
    
    [Flags]
    public enum SettingFlags
    {
        NONE = 0,
        SERIALIZED
    }
    
    
}