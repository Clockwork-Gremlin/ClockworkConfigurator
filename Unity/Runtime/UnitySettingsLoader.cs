using ClockworkConfigurator.Core;
using UnityEngine;

namespace ClockworkConfigurator.Unity
{
    public static class UnitySettingsLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnLoad()
        {
            Application.quitting += OnQuit;
            Configurator.OnStart();
            Configurator.LogWarning += Debug.LogWarning;
            Configurator.LogError += Debug.LogError;
            Configurator.Log += Debug.Log;
            Configurator.Load(Application.persistentDataPath + "/Settings", "config.json");
        }

        private static void OnQuit()
        {
            Configurator.OnQuit();
            Configurator.Save(Application.persistentDataPath + "/Settings", "config.json");
        }
    }
}
