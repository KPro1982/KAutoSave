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
            if (CheckTime(PersistentData.AutoSaveFrequency))
            {
                SaveAll();
            }
        }

        if (IsAutoRecoverEnabled)
        {
            if (CheckTime(PersistentData.AutoRecoverFrequency))
            {
                SaveAutoRecover();
            }
        }
    }



    static void HandleProjectChanged()
    {
        SaveAfter(PersistentData.AutoSaveFrequency*60);
    }
        

    private static void HandlePlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SaveAfter(PersistentData.AutoSaveFrequency*60);
        }
    }

    private static void TimedSave()
    {
        Debug.Log("Timed Saved!!!");
        SaveAll();
    }
    public static void SaveAfter(int seconds)
    {
        if (CheckTime(seconds))
        {
            SaveAll();

        }
    }

    private static bool CheckTime(int seconds)
    {
        DateTime currentTime = DateTime.Now;
        DateTime lastSaved = PersistentData.LastSavedTime;
        TimeSpan elapsedTime = currentTime.Subtract(lastSaved);
        if (elapsedTime.TotalSeconds >= seconds)
        {
            Debug.Log($"Time since last successful check: {elapsedTime.TotalSeconds}");
            return true;
        }

        return false;
    }

    private static void SaveAll()
    {
        PersistentData.LastSavedTime = DateTime.Now;
        // EditorSceneManager.MarkAllScenesDirty();
        /*EditorSceneManager.SaveOpenScenes();*/
        EditorApplication.ExecuteMenuItem("File/Save Project");
        Debug.Log("File/Save Executed");
        EditorApplication.ExecuteMenuItem("File/Save");
        Debug.Log("File/Project Save Executed");

   
    }
    
    static void SaveAutoRecover() {
        if ( !System.IO.Directory.Exists( Application.dataPath + "/" + "AutoRecover" ) ) {
            System.IO.Directory.CreateDirectory( Application.dataPath + "/" + "AutoRecover"  );
            AssetDatabase.Refresh();
        }

        string sceneName = EditorSceneManager.GetActiveScene().name;
        string savePath = Application.dataPath + "/" + "AutoRecover/" + sceneName + "_" +
                          DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".unity";
        
        EditorSceneManager.SaveScene( EditorSceneManager.GetActiveScene() , savePath , true );
        Debug.Log( "AutoRecover version saved!");
    }
        

}