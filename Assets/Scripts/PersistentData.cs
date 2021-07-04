
using System;
using UnityEngine;
using UnityEditor;

namespace KAutoSave
{
    public class PersistentData : EditorWindow
    {
        private static bool _enabled = false;
        private static DateTime _lastSavedTime;
        private static int _autoSaveFrequency = 1; // minutes
        private static void SetVariable(string _Name, string _Value)
        {
            EditorPrefs.SetString(_Name, _Value);
        }
        
        private static string GetVariable(string _key)
        {
            if (EditorPrefs.HasKey(_key))
                return EditorPrefs.GetString(_key);

            return "";
        }
        
        public static float EditorTimeInSeconds {
            get { return (float)(EditorApplication.timeSinceStartup % 1000000); }
        }

        public static bool Enabled
        {
            get
            {
                bool isEnabled;
                string strEnabled = GetVariable("Enabled");
                if (bool.TryParse(strEnabled, out isEnabled))
                {
                    return isEnabled;
                }
                return true; 
                
            }
            set
            {
                _enabled = (bool)value;
                SetVariable("Enabled", _enabled.ToString());
            }
        }

        public static DateTime LastSavedTime
        {
            get
            {
                DateTime tempLastSavedTime;
                string strLastSavedTime= GetVariable("LastSavedTime");
                if (DateTime.TryParse(strLastSavedTime, out tempLastSavedTime))
                {
                    return tempLastSavedTime;
                }
                return DateTime.Now;
            }
            set
            {
                _lastSavedTime = value;
                SetVariable("LastSavedTime", _lastSavedTime.ToShortTimeString());
            }
        }

        public static int AutoSaveFrequency
        {
            get
            {
                int tempAutoSaveFrequency;
                string strAutoSaveFrequency = GetVariable("AutoSaveFrequency");
                if (int.TryParse(strAutoSaveFrequency, out tempAutoSaveFrequency))
                {
                    return tempAutoSaveFrequency;
                }

                return 1;

            }
            set
            {
                _autoSaveFrequency = value ;
                SetVariable("AutoSaveFrequency", _autoSaveFrequency.ToString());
            }
        }
    }
}