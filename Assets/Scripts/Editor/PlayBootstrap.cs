#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayBootstrap
{
    static PlayBootstrap()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            Debug.Log("Entering");
            string scenePath = SceneUtility.GetScenePathByBuildIndex(0);
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        }
    }
}
#endif


