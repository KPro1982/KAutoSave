using System;
using System.Timers;
using Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSave : EditorWindow
{
    private static bool _isAutoSaveEnabled = false;
    private static bool _isAutoRecoverEnabled = false;

    static AutoSave()
    {
        EditorApplication.update += HandleUpdate;
        EditorApplication.projectChanged += HandleProjectChanged;
        EditorApplication.playModeStateChanged += HandlePlayModeState;
        IsAutoSaveEnabled = PersistentData.AutoSaveEnabled;
        IsAutoRecoverEnabled = PersistentData.AutoRecoverEnabled;

    }

    public static bool IsAutoSaveEnabled
    {
        get => _isAutoSaveEnabled;
        set => _isAutoSaveEnabled = value;
    }

    public static bool IsAutoRecoverEnabled
    {
        get => _isAutoRecoverEnabled;
        set => _isAutoRecoverEnabled = value;
    }


    public static void HandleUpdate()
    {
        if (IsAutoSaveEnabled)
        {
            if (CheckLastSaveTime(PersistentData.AutoSaveFrequency*60))
            {
                SaveAll();
            }
        }

        if (IsAutoRecoverEnabled)
        {
            if (CheckLastAutoRecoverTime(PersistentData.AutoRecoverFrequency*60))
            {
                SaveAutoRecover();
            }
        }
    }


    static void HandleProjectChanged()
    {
        SaveAfter(60);
    }


    private static void HandlePlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SaveAfter(60);
        }
    }

    private static void TimedSave()
    {
        SaveAll();
    }

    public static void SaveAfter(int seconds)
    {
        if (CheckLastSaveTime(seconds))
        {
            SaveAll();
        }
    }

    private static bool CheckLastSaveTime(int seconds)
    {
        DateTime currentTime = DateTime.Now;
        DateTime lastSaved;
        if (!PersistentData.TryGetLastSavedTime(out lastSaved))
        {
            return true;
        }


        TimeSpan elapsedTime = currentTime.Subtract(lastSaved);
        if (elapsedTime.TotalSeconds >= seconds)
        {
            return true;
        }

        return false;
    }

    private static bool CheckLastAutoRecoverTime(int seconds)
    {
        DateTime currentTime = DateTime.Now;
        DateTime lastSaved;
        if (!PersistentData.TryGetLastSavedTime(out lastSaved))
        {
            return true;
        }

        TimeSpan elapsedTime = currentTime.Subtract(lastSaved);
        if (elapsedTime.TotalSeconds >= seconds)
        {
            return true;
        }

        return false;
    }

    private static void SaveAll()
    {

        if ((float) (EditorApplication.timeSinceStartup % 1000000) > 30)
        {
            EditorApplication.ExecuteMenuItem("File/Save Project");
            EditorApplication.ExecuteMenuItem("File/Save");
            Debug.Log("Saved!");
            PersistentData.LastSavedTime = DateTime.Now;
        }
        else
        {
            Debug.Log("Skipping initial save on application open.");
        }
    }

    static void SaveAutoRecover()
    {
        if (!System.IO.Directory.Exists(Application.dataPath + "/" + "AutoRecover"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/" + "AutoRecover");
            AssetDatabase.Refresh();
        }

        string sceneName = EditorSceneManager.GetActiveScene().name;
        string savePath = Application.dataPath + "/" + "AutoRecover/" + sceneName + "_" +
                          DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".unity";

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), savePath, true);
        PersistentData.LastAutoRecoverTime = DateTime.Now;
        Debug.Log("AutoRecover!");
    }
}