using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SessionInspector : EditorWindow
{
    [MenuItem("Tools/Session Inspector")]
    private static void Init()
    {
        var panel = (MasterPanel)GetWindow(typeof(SessionInspector));
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
        
        GUILayout.Label("Session Inspector", headerStyle);
        GUILayout.Space(20);
        
        if (Application.isPlaying)
        {
            // Play mode
            var sessionID = Platform.HasInstance && Platform.Instance.Session != null && 
                            Platform.Instance.Session.ID != Guid.Empty
                ? Platform.Instance.Session.ID.ToString()
                : "No session";
            
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Session ID");
                EditorGUILayout.SelectableLabel(sessionID, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            GUILayout.EndHorizontal();

            if (sessionID == "No session")
                return;
            
            GUILayout.Space(20);
            var sessionObjectsListText = "";
            foreach (var obj in Platform.Instance.Session!.GetAllSessionObjects())
                sessionObjectsListText += $"{obj.name}: {obj.GetType()} - {obj.ID}\n";
            sessionObjectsListText = sessionObjectsListText[..^1];
            EditorGUILayout.LabelField("Session Objects");
            EditorGUILayout.SelectableLabel(sessionObjectsListText, EditorStyles.textField);
        }
        else
        {
            GUILayout.Label("Enter playmode to use Session Inspector", EditorStyles.centeredGreyMiniLabel);
        }
    }
}
