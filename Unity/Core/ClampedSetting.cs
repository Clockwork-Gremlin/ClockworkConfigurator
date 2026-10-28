using System;

namespace ClockworkConfigurator.Core
{
    public class ClampedSetting<T> : Setting<T> where T : IComparable, IConvertible
    {
        public T Minimum { get; private set; }
        public T Maximum { get; private set; }
        
        public ClampedSetting(string group, string name, string description, T startingValue, T minimumValue, T maximumValue,
            SettingFlags flags = SettingFlags.NONE) : base(group, name, description, startingValue, flags)
        {
            Minimum = minimumValue;
            Maximum = maximumValue;
        }

        public override Setting<T> Set(T newValue, bool asServer = false)
        {
            if (newValue is IComparable c)
            {
                if (c.CompareTo(Minimum) < 0)
                    c = Minimum;
                if (c.CompareTo(Maximum) > 0)
                    c = Maximum;
                
                newValue = (T)c;
            }
            
            return base.Set(newValue);
        }
    }
}