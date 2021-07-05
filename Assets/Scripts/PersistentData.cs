using System;
using UnityEditor;
using UnityEngine;

public class PersistentData : EditorWindow
{
    private static bool _autoSaveEnabled = false;
    private static bool _autoRecoverEnabled = false;
    private static DateTime _lastSavedTime;
    private static DateTime _lastAutoRecoverTime;
    private static int _autoSaveFrequency = 1; // minutes
    private static int _autoRecoverFrequency = 10; // minutes
    private static void SetValue(string _key, string _value)
    {
        EditorPrefs.SetString(_key, _value);
    }
        
    private static string GetValue(string _key)
    {
        if (EditorPrefs.HasKey(_key))
            return EditorPrefs.GetString(_key);

        return "";
    }
        
    public static float EditorTimeInSeconds {
        get { return (float)(EditorApplication.timeSinceStartup % 1000000); }
    }

    public static bool AutoSaveEnabled
    {
        get
        {
            bool isEnabled;
            string strEnabled = GetValue("AutoSaveEnabled");
            if (bool.TryParse(strEnabled, out isEnabled))
            {
                return isEnabled;
            }
            return true; 
                
        }
        set
        {
            _autoSaveEnabled = (bool)value;
            SetValue("AutoSaveEnabled", _autoSaveEnabled.ToString());
            AutoSave.IsAutoSaveEnabled = value;
        }
    }

    public static DateTime LastSavedTime
    {

        set
        {
            _lastSavedTime = value;
            SetValue("LastSavedTime", _lastSavedTime.ToLongTimeString());
        }
    }

    
    
    public static bool TryGetLastSavedTime(out DateTime _lastSavedTime)
    {
        DateTime tempLastSavedTime;
        string strLastSavedTime = GetValue("LastSavedTime");
        if (strLastSavedTime.Length > 0)
        {
            if (DateTime.TryParse(strLastSavedTime, out tempLastSavedTime))
            {
                _lastSavedTime = tempLastSavedTime;
                return true;
            }
        }

        _lastSavedTime = DateTime.Now;
        return false;
    }
    public static string GetLastSaveTimeStr()
    {
        DateTime tempLastSavedTime;
        string strLastSavedTime= GetValue("LastSavedTime");
        if (strLastSavedTime.Length > 0)
        {
            if (DateTime.TryParse(strLastSavedTime, out tempLastSavedTime))
            {
                return tempLastSavedTime.ToString();
            }
        }

        return "";
    }
    
    public static int AutoSaveFrequency
    {
        get
        {
            int tempAutoSaveFrequency;
            string strAutoSaveFrequency = GetValue("AutoSaveFrequency");
            if (int.TryParse(strAutoSaveFrequency, out tempAutoSaveFrequency))
            {
                return tempAutoSaveFrequency;
            }

            return 1;

        }
        set
        {
            _autoSaveFrequency = value ;
            SetValue("AutoSaveFrequency", _autoSaveFrequency.ToString());
        }
    }

    public static int AutoRecoverFrequency
    {
        get
        {
            int tempAutoRecoverFrequency;
            string strAutoRecoverFrequency = GetValue("AutoRecoverFrequency");
            if (int.TryParse(strAutoRecoverFrequency, out tempAutoRecoverFrequency))
            {
                return tempAutoRecoverFrequency;
            }

            return 1;

        }
        set
        {
            _autoRecoverFrequency = value ;
            SetValue("AutoRecoverFrequency", _autoRecoverFrequency.ToString());
        }
    }

    public static bool AutoRecoverEnabled
    {
        get
        {
            bool isEnabled;
            string strEnabled = GetValue("AutoRecoverEnabled");
            if (bool.TryParse(strEnabled, out isEnabled))
            {
                return isEnabled;
            }
            return true; 
                
        }
        set
        {
            _autoRecoverEnabled = (bool)value;
            SetValue("AutoRecoverEnabled", _autoRecoverEnabled.ToString());
            AutoSave.IsAutoSaveEnabled = value;
        }
    }

    public static DateTime LastAutoRecoverTime
    {

        set
        {
            _lastAutoRecoverTime = value;
            SetValue("LastAutoRecoverTime", _lastAutoRecoverTime.ToLongTimeString());
        }
    }
    
    
    public static bool TryGetLastAutoRecoverTime(out DateTime _lastAutoRecoverTime)
    {
        DateTime tempLastAutoRecoverTime;
        string strLastAutoRecoverTime= GetValue("LastAutoRecoverTime");
        if (strLastAutoRecoverTime.Length > 0)
        {
            if (DateTime.TryParse(strLastAutoRecoverTime, out tempLastAutoRecoverTime))
            {
                _lastAutoRecoverTime =  tempLastAutoRecoverTime;
                return true;
            }
                
        }

        _lastAutoRecoverTime = DateTime.Now;
        return false;
    }
}