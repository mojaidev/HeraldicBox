using Mojai.Libraries.UI;
using UnityEngine;
using NCMS.Utils;

namespace HeraldicBox
{
    class HeraldicBoxTab
    {
        public static void Setup()
        {
            TabLibrary.Tab tab = new TabLibrary.Tab("tab_heraldicbox", "HeraldicBox", Resources.Load<Sprite>("ui/icons/tab_heraldic"), new Vector2(1f, 1f), new Vector2(193, 49.62f));
            PowerButton newfamily_button = PowerButtons.CreateButton("heraldic_newfamily", Resources.Load<Sprite>("ui/icons/new_family_icon"), "New Family", "Create a new famliy by dropping this.", Vector2.zero);
            PowerButton settings_button = PowerButtons.CreateButton("heraldic_settings", Resources.Load<Sprite>("ui/icons/options_icon"), "Settings", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero);

            TabLibrary.Tab.AddButtonToTab(newfamily_button, "tab_heraldicbox", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(settings_button, "tab_heraldicbox", new Vector2(254.2f, 18));
        }
    }
}
