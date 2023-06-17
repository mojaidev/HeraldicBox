using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace HeraldicBox
{
    class HeraldicBoxSettings
    {
        [NonSerialized] public static HeraldicBoxSettings instance;


        public class Setting
        {
            public string configName;
            public object obj;

            public Setting(object setting, string publicName = "")
            {
                configName = publicName;
                obj = setting;
            }

            public void switchBoolConfig()
            {
                if(obj is bool)
                {
                    if((bool)obj == true)
                    {
                        obj = false;
                    }
                    else
                    {
                        obj = true;
                    }
                }

                SaveSettings();
            }
        }


        // ====================================================
        // DEFAULT SETTINGS
        // ====================================================





        public Dictionary<string, Setting> settings = new Dictionary<string, Setting>() {
            { "Asexual_Reproduction", new Setting(true, "Asexual Reproduction")},
            { "LGBT_Reproduction", new Setting(true, "LGBT Reproduction") },
            { "tab_HeraldicBox_Position", new Setting((float)-400) }
        };




        /*
        public static object SetSetting(string name, object value)
        {
            return instance.settings[name].obj = value;
        }
        */

        public static object GetSetting(string name, Type convertTo = null)
        {
            if(convertTo != null)
            {
                return Convert.ChangeType(instance.settings[name].obj, convertTo); ;
            }
            else
            {
                return instance.settings[name].obj;
            }
        }

        public static void Setup()
        {
            if (File.Exists($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json"))
            {
                string settingsjson = File.ReadAllText($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json");
                HeraldicBoxSettings savedInstance = JsonConvert.DeserializeObject<HeraldicBoxSettings>(settingsjson);
                Mojai.Mod.Util.Print(settingsjson);
                instance = savedInstance;
            }
            else
            {
                HeraldicBoxSettings defaultSettings = new HeraldicBoxSettings();

                string defaultSettingsJson = JsonConvert.SerializeObject(defaultSettings);
                File.WriteAllText($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json", defaultSettingsJson);

                instance = defaultSettings;
            }
        }

        public static void SaveSettings()
        {
            string saveSettingsJson = JsonConvert.SerializeObject(instance);
            File.WriteAllText($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json", saveSettingsJson);
        }
    }
}
