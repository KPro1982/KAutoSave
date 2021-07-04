using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

namespace KAutoSave
{
    public class AutoSaveWindow : EditorWindow
    {

        [MenuItem("Window/AutoSave")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            PersistentData.LastSavedTime = DateTime.Now;
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            EditorWindow window =
                GetWindow<AutoSaveWindow>("AutoSave", new Type[] {inspectorType});
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label($"Last Saved: {PersistentData.LastSavedTime}", EditorStyles.boldLabel);

           PersistentData.Enabled = EditorGUILayout.Toggle("Enable", PersistentData.Enabled);
            PersistentData.AutoSaveFrequency =
                (int)EditorGUILayout.Slider("Frequency (minutes):", PersistentData.AutoSaveFrequency, 1, 60);

        }
    }
}