using System;
using System.Collections.Generic;

namespace ClockworkConfigurator.Core
{
    
    public class Setting <T> : SettingBase where T : IConvertible
    {

        public override string Id {get;}
        public override string GroupId {get;}

        public readonly string DisplayDescription;
        public T Value {get; protected set;}
        public T DefaultValue {get; protected set;}
        public T TemporaryValue {get; protected set;}
        private readonly SettingFlags _flags;

        public Action<T> OnSettingChanged { get; set; }
        public Action<T> OnSettingApplied { get; set; }

        public Setting(string groupID, string name, string description, T startingValue, SettingFlags flags = SettingFlags.NONE)
        {
            _flags = flags;
            Value = startingValue;
            DefaultValue = startingValue;
            GroupId = groupID.ToLower();
            Id = name.ToLower().Replace(' ', '_');
            DisplayDescription = description;
        }

        public virtual Setting<T> Set(T newValue, bool asServer = false)
        {
            TemporaryValue = newValue;
            OnSettingChanged?.Invoke(TemporaryValue);
            return this;
        }

        public override void Default()
        {
            Value = DefaultValue;
            TemporaryValue = DefaultValue;
            OnSettingChanged?.Invoke(Value);
            OnSettingApplied?.Invoke(Value);
        }
        
        public override void Reset()
        {
            TemporaryValue = Value;
            OnSettingChanged?.Invoke(Value);
            OnSettingApplied?.Invoke(Value);
        }
        
        public override void Apply(bool send = true)
        {
            Value = TemporaryValue;
            OnSettingApplied?.Invoke(Value);
        }

        public override void ParseUnsafe(object valueToParse)
        {
            try
            {
                Set((T)Convert.ChangeType(valueToParse, typeof(T)), true).Apply(false);
            }
            catch (Exception e)
            {
                Configurator.LogWarning($"{e.GetType()}! Settings contained definition for {Id} ({valueToParse}), but failed to parse value to corresponding type {typeof(T)}. Reverting to default value.");
            }
        }

        public override object GetWrappedValue()
        {
            return Value;
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