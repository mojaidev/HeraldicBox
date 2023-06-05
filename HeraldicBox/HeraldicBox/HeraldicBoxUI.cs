﻿using Mojai.Libraries.UI;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine.UI;
using System;

namespace HeraldicBox
{
    // ====================================================
    // HERALDICBOXUI
    // I hate writing ui code
    // ====================================================
    class HeraldicBoxUI
    {
        public static void SetupAll()
        {
            tab_setup();
        }

        private static void tab_setup()
        {
            // ====================================================
            //
            //            Tab section is right down
            //
            //___________   _________________   __________________
            //   Mojai   \_/   HeraldicBox   \_/   Random Stuff   \
            //
            // ====================================================

            TabLibrary.Tab tab = new TabLibrary.Tab("tab_heraldicbox", "HeraldicBox", Resources.Load<Sprite>("ui/icons/tab_heraldic"), new Vector2(1f, 1f), new Vector2(193, 49.62f));
            PowerButton newfamily_button = PowerButtons.CreateButton("heraldic_newfamily_drop", Resources.Load<Sprite>("ui/icons/new_family_icon"), "New Family", "Create a new famliy by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton inspectfamily_drop_button = PowerButtons.CreateButton("heraldic_inspectfamily_drop", Resources.Load<Sprite>("ui/icons/inspect_icon"), "Inspect Family", "Get a unit's family tree by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton settings_button = PowerButtons.CreateButton("heraldic_settings", Resources.Load<Sprite>("ui/icons/options_icon"), "Settings", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero);

            TabLibrary.Tab.AddButtonToTab(newfamily_button, "tab_heraldicbox", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(inspectfamily_drop_button, "tab_heraldicbox", new Vector2(254.2f, 18));
            TabLibrary.Tab.AddButtonToTab(settings_button, "tab_heraldicbox", new Vector2(297.2f, 18));
        }

        public class inspect_family_window
        {
            // ====================================================
            // Inspect Family window is over here!!
            //
            //  __________________________________________________
            // |                 This dude family                 |
            // |                    _______                       |
            // |                   |       |                      |
            // |                   |   |   |                      |
            // |                   |   |   |                      |
            // |                   |  o|o  |                      |
            // |                   |_______|                      |
            // |                                                  |
            // |                                                  |
            // |       Parents:                                   |
            // |                                                  |
            // |                                                  |
            // |       Childs:                                    |
            // |__________________________________________________|
            //
            // WARNING: BAD CODE BELOW!!
            // ====================================================

            private static inspect_family_window openedWindow;
            private WindowLibrary.EasyScrollWindow window;
            private HeraldicInfo referenced;

            public static void clickHide_Postfix(ScrollWindow __instance)
            {
                if(openedWindow != null && __instance == openedWindow.window.scrollWindow)
                {
                    openedWindow = null;
                }
            }

            private class avatarButton
            {
                public PowerButton button;
                private inspect_family_window window;
                private HeraldicInfo info;

                void inspect_family_window_button()
                {
                    // scrollWindow.hide() is better but ill use the postfixed one by now
                    if(window.referenced == info)
                    {
                        if(window.referenced.actor != null)
                        {
                            window.window.scrollWindow.clickHide();
                            Config.selectedUnit = info.actor;
                            ScrollWindow.moveAllToLeftAndRemove(true);
                            ScrollWindow.showWindow("inspect_unit");
                        }
                    }
                    else
                    {
                        window.window.scrollWindow.clickHide();
                        new inspect_family_window(info);
                    }
                }

                public avatarButton(HeraldicInfo pInfo, inspect_family_window pWindow, Vector2 pos, float size = 32) {
                    info = pInfo;
                    window = pWindow;
                    String button_uuid = Guid.NewGuid().ToString();
                    if (pInfo.actor != null)
                    {
                        ActorData actor_data = Reflection.GetField(typeof(Actor), info.actor, "data") as ActorData;

                        if(info.actor.city != null)
                        {
                            button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, null, info.actor.getName(), "Living | " + info.actor.city.getCityName(), pos, ButtonType.Click, window.window.content.transform, inspect_family_window_button);
                        }
                        else
                        {
                            button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, null, info.actor.getName(), "Living | Nowhere", pos, ButtonType.Click, window.window.content.transform, inspect_family_window_button);
                        }
                       
                        GameObject loader_object = new GameObject("loader");
                        //loader_object.transform.parent = window.content.transform; // <-- Debugging puroposes
                        UnitAvatarLoader loader = loader_object.AddComponent<UnitAvatarLoader>() as UnitAvatarLoader;
                        loader.load(info.actor);

                        Image avatar = loader.transform.GetChild(0).gameObject.GetComponent<Image>();

                        button.gameObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = avatar.sprite;

                        button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size, size, 0);

                        /*
                        Image button_background = button.GetComponent<Image>(); 
                        button_background.enabled = false;
                        */
                    }
                    else
                    {
                        button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, Resources.Load<Sprite>("ui/icons/dead"), pInfo.actorName, "Dead", pos, ButtonType.Click, window.window.content.transform, inspect_family_window_button);
                    }
                }
            }

            public inspect_family_window(HeraldicInfo pInfo)
            {
                if(openedWindow == null)
                {
                    referenced = pInfo;

                    // Yes you are right! this will create multiple windows that are not going to be deleted
                    // this should be fixed but its ok for now since i dont think someone will open like 10000 windows.

                    String window_uuid = Guid.NewGuid().ToString();
                    window = new WindowLibrary.EasyScrollWindow("inspect_family_window_" + window_uuid, "Family Inspector");

                    avatarButton portrait = new avatarButton(pInfo, this, new Vector2(100, -30), 45);

                    if (pInfo.father != null)
                    {
                        new avatarButton(pInfo.father, this, new Vector2(80, -80));
                    }

                    if (pInfo.mother != null)
                    {
                        new avatarButton(pInfo.mother, this, new Vector2(115, -80));
                    }

                    window.UpdateVerticalRect((float)220);

                    float lastX = 40;
                    float lastY = -150;
                    float lastVerticalRectMinus = -220;
                    float lastVerticalRect = 220;

                    foreach (HeraldicInfo child in pInfo.childs)
                    {
                        new avatarButton(child, this, new Vector2(lastX, lastY));
                        if (lastX < 145)
                        {
                            lastX += 35;
                        }
                        else
                        {
                            lastX = 40;
                            lastY += -35;
                        }

                        if (lastY == lastVerticalRectMinus)
                        {
                            lastVerticalRectMinus += -35;
                            lastVerticalRect += 35;
                            window.UpdateVerticalRect(lastVerticalRect);
                        }

                    }

                    // I dont know why but when i put the text before UpdateVerticalRect the text x size is updated to 90, WTF?

                    WindowLibrary.AddTextToObject(window.content, "Parents: ", 10, new Vector3(40, -70, 0), window.scrollWindow.name);
                    WindowLibrary.AddTextToObject(window.content, "Childs: ", 10, new Vector3((float)36.30, -120, 0), window.scrollWindow.name);

                    window.scrollWindow.show();
                    openedWindow = this;
                }
            }
        }
    }
}
