using System;
using System.Collections.Generic;

namespace ClockworkConfigurator.Core
{
    public abstract class SettingBase
    {
        public abstract string Id {get;}
        public abstract string GroupId {get;}
        public abstract object GetWrappedValue();
        public abstract bool HasFlag(SettingFlags flag);
        public abstract void ParseUnsafe(object valueToParse);
        public abstract void Default();
        public abstract void Reset();
        public abstract void Apply(bool send = true);
    }
}