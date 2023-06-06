using Mojai.Libraries.UI;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;
using Mojai.Libraries.Other;

namespace HeraldicBox
{
    class Test
    {
        public static void debug_1()
        {
            Saves.LoadHeraldic();
        }

        public static void debug_2()
        {
            Saves.SaveHerladic();
        }

        public static void Init()
        {
            TabLibrary.Tab debugTab = new TabLibrary.Tab("tab_debug_heraldic", "HeraldicBox [DEBUG TAB]", Resources.Load<Sprite>("ui/icons/new_icon"), new Vector2(1f, 1f), new Vector2(150, 49.62f));
            PowerButton debugButton1 = PowerButtons.CreateButton("debugdrop1", Resources.Load<Sprite>("ui/icons/new_icon"), "LoadHeraldic()", "LoadHeraldic()", Vector2.zero, ButtonType.Click, null, debug_1);
            PowerButton debugButton2 = PowerButtons.CreateButton("debugdrop2", Resources.Load<Sprite>("ui/icons/new_icon"), "SaveHerladic()", "SaveHerladic()", Vector2.zero, ButtonType.Click, null, debug_2);
            TabLibrary.Tab.AddButtonToTab(debugButton1, "tab_debug_heraldic", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(debugButton2, "tab_debug_heraldic", new Vector2(311.2f, 18));
        }
    }
}