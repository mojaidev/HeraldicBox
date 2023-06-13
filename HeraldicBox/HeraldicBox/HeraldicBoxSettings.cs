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

        // ====================================================
        // BEHAVIOUR SETTINGS
        // ====================================================

        public bool Asexual_Reproduction = true;
        public bool LGBT_Reproduction = true;

        // ====================================================
        // UI SETTINGS
        // ====================================================

        public float tab_HeraldicBox_Position = -400;

        // ====================================================
        // SETTINGS MANAGER
        // ====================================================

        public static void Setup()
        {
            if (File.Exists($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json"))
            {
                string settingsjson = File.ReadAllText($"{NCMS.Core.NCMSModsPath}/HeraldicBoxSettings.json");
                HeraldicBoxSettings savedInstance = JsonConvert.DeserializeObject<HeraldicBoxSettings>(settingsjson);
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
