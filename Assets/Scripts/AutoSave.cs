using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace KAutoSave
{
    [InitializeOnLoad]
    public static class AutoSave
    {
        static AutoSave()
        {
            EditorApplication.projectChanged += HandleProjectChanged;
            EditorApplication.playModeStateChanged += HandlePlayModeState;
        }

        static void HandleProjectChanged()
        {
            SaveAfter(PersistentData.AutoSaveFrequency);
        }

        private static void HandlePlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SaveAfter(PersistentData.AutoSaveFrequency);
            }
        }

        private static void SaveAfter(int seconds)
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastSaved = PersistentData.LastSavedTime;
            TimeSpan elapsedTime = currentTime.Subtract(lastSaved);
            Debug.Log($"Elapsed Time: {elapsedTime.TotalSeconds}");
            if (elapsedTime.TotalSeconds >= seconds)
            {
                SaveAll();
            }
        }

        private static void SaveAll()
        {
            PersistentData.LastSavedTime = DateTime.Now;
            // EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }
    }
}