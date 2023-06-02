using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mojai;
using System.Threading;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;

namespace Mojai.Managers
{
    public class ScriptManager
    {
        private static List<MonoBehaviour> scripts = new List<MonoBehaviour>();

        public static void RemoveManagment(MonoBehaviour script)
        {
            scripts.Remove(script);
        }

        /// <summary>
        /// This will make the MonoBehaviour Update() method be
        /// called every second.
        /// 
        /// idk if its the best solution but its ok... i think
        /// </summary>
        public static void ManageScript(MonoBehaviour script)
        {
            scripts.Add(script);
        }

        public static void UpdateAll()
        {
            foreach(MonoBehaviour script in scripts)
            {
                script.CallMethod("Update");
            }
        }
    }
}
