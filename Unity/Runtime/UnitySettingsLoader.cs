using ClockworkConfigurator.Core;
using UnityEngine;

namespace ClockworkConfigurator.Unity
{
    public static class UnitySettingsLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnLoad()
        {
            Application.quitting += OnQuit;
            Configurator.OnStart();
            Configurator.LogWarning += Debug.LogWarning;
            Configurator.LogError += Debug.LogError;
            Configurator.Log += Debug.Log;
        }

        private static void OnQuit()
        {
            Configurator.OnQuit();
        }
    }
}