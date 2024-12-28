#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayBootstrap
{
    [SerializeField]
    private static bool _online = true;
    static PlayBootstrap()
    {
        Debug.Log($"PlayBootstrap");
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (_online)
        {
            Debug.Log($"PlayBootstrap.OnPlayModeChanged {state}");

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Debug.Log("Entering");
                string scenePath = SceneUtility.GetScenePathByBuildIndex(0);
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            }
        }
    }
}
#endif


