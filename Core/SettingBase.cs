using System;
using System.Collections.Generic;

namespace ClockworkConfigurator.Core
{
    /// <summary>
    /// some stupid idiot (me) names this 'base'
    /// did so because fishnet won't serialize generics AT ALL
    /// </summary>
    public abstract class SettingBase
    {
        public abstract string ID();
        public abstract string GroupId();
        public abstract object GetWrappedValue();
        public abstract bool HasFlag(SettingFlags flag);
        public abstract void ParseUnsafe(object valueToParse);
        public abstract void Write(Dictionary<string,dynamic> storageDict);
        public abstract void Default();
        public abstract void Reset();
        public abstract void Apply(bool send = true);
    }
}