using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterPanel : EditorWindow
{
    [MenuItem("Tools/Master Panel")]
    private static void Init()
    {
        var panel = (MasterPanel)GetWindow(typeof(MasterPanel));
        panel.Show();
    }

    private void OnGUI()
    {
        var headerStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal =
            {
                textColor = Color.white
            },
            fontSize = 40
        };

        GUILayout.Label("Master Panel", headerStyle);
        GUILayout.Space(20);

        if (Application.isPlaying)
        {
            // Play mode
        }
        else
        {
            // Editor
            
            if (GUILayout.Button("Play"))
            {
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
                EditorApplication.EnterPlaymode();
            }
        }
        
        GUILayout.Space(20);
        GUILayout.Label("SAVE MANAGEMENT", EditorStyles.boldLabel);
        if (GUILayout.Button("Open saves folder"))
            Application.OpenURL(Globals.Instance.SavePath);
    }
}
