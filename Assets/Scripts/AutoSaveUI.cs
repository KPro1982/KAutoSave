using System;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class AutoSaveUI : EditorWindow
{
    [MenuItem("Window/AutoSave")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        /*PersistentData.LastSavedTime = DateTime.Now;*/
        Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
        EditorWindow window =
            GetWindow<AutoSaveUI>("AutoSave", new Type[] {inspectorType});
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Space(5);
        // GUILayout.Label($"AutoSave", EditorStyles.boldLabel);

        PersistentData.AutoSaveEnabled = EditorGUILayout.BeginToggleGroup ("AutoSave", PersistentData.AutoSaveEnabled);
        PersistentData.AutoSaveFrequency =
            (int)EditorGUILayout.Slider("AutoSave (minutes):", PersistentData.AutoSaveFrequency, 1, 30);
        GUILayout.Label($"Last Saved: {PersistentData.GetLastSaveTimeStr()}");
        EditorGUILayout.EndToggleGroup();
        
        GUILayout.Space(20);
        // GUILayout.Label($"AutoRecover", EditorStyles.boldLabel);
        
        PersistentData.AutoRecoverEnabled = EditorGUILayout.BeginToggleGroup ("AutoRecover", PersistentData.AutoRecoverEnabled);
        PersistentData.AutoRecoverFrequency =
            (int)EditorGUILayout.Slider("AutoRecover (minutes):", PersistentData.AutoRecoverFrequency, 1, 60);
        
        string strLastAuto = "";
        DateTime _LastAuto;
        if (PersistentData.TryGetLastAutoRecoverTime(out _LastAuto))
        {
            strLastAuto = _LastAuto.ToString();
        }
        else
        {
            strLastAuto = "";
        }
        
        GUILayout.Label($"Last AutoRecover: {strLastAuto}");
        EditorGUILayout.EndToggleGroup();
    }
}