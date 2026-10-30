using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClockworkConfigurator.Core
{
    public interface ISettingsManager
    {
        public string FileName {get;}
        public string GetFilePath();

        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
    }
}