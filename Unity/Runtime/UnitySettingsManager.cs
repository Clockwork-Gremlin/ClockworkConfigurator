using ClockworkConfigurator.Core;
using UnityEngine;

namespace ClockworkConfigurator.Unity
{
    [DefaultExecutionOrder(-1000)]
    public class UnitySettingsManager : MonoBehaviour, ISettingsManager
    {

        [field: SerializeField] public string FileName {get; private set;}
        private static UnitySettingsManager instance;

        private void Awake() 
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            Configurator.SettingsManager = this;
            Configurator.Load(); 
        }

        private void OnApplicationQuit() 
        {
            Configurator.Save();    
        }
        
        public string GetFilePath() => Application.persistentDataPath;

        public void Log (string message) => Debug.Log(message);
        public void LogWarning (string message) => Debug.LogWarning(message);
        public void LogError (string message) => Debug.LogError(message);
    }
}
