using UnityEngine;

namespace Unrez.Essential
{
    /// <summary>
    /// Singleton de Acesso Pregui�oso e Auto-Cria��o.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _shuttingDown = false;
        private static readonly object _lock = new();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    Debug.LogWarning($"[Singleton] Acesso � inst�ncia '{typeof(T).Name}' ap�s o desligamento. Retornando null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindAnyObjectByType(typeof(T));

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).Name + "_SingletonInstance";
                        }

                        // Garante a persist�ncia do objeto
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    return _instance;
                }
            }
        }

        // Protege contra duplica��o
        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            _shuttingDown = true;
        }
    }
}