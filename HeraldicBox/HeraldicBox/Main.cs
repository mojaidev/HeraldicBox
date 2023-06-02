using System;
using NCMS;
using UnityEngine;
using ReflectionUtility;

namespace FirstPersonBox
{
    [ModEntry]
    class Main : MonoBehaviour
    {
        void Awake()
        {
            HeraldicBox.HeraldicBox.Start();
        }

        void Update()
        {
            Mojai.Managers.ScriptManager.UpdateAll();
        }
    }
}
