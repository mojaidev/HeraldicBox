using Mojai.Libraries.UI;
using System.Threading;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace HeraldicBox
{
    // ====================================================
    // HERALDICBOXUI
    // I hate writing ui code
    // ====================================================
    class HeraldicBoxUI
    {
        public static void Setup()
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
            PowerButton newfamily_button = PowerButtons.CreateButton("heraldic_newfamily", Resources.Load<Sprite>("ui/icons/new_family_icon"), "New Family", "Create a new famliy by dropping this.", Vector2.zero);
            PowerButton settings_button = PowerButtons.CreateButton("heraldic_settings", Resources.Load<Sprite>("ui/icons/options_icon"), "Settings", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero);

            TabLibrary.Tab.AddButtonToTab(newfamily_button, "tab_heraldicbox", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(settings_button, "tab_heraldicbox", new Vector2(254.2f, 18));
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

            private WindowLibrary.EasyScrollWindow window;
            private HeraldicInfo referenced;

            private class avatarButton
            {
                public PowerButton button;
                private inspect_family_window window;
                private Actor actor;

                void inspect_family_window_button()
                {
                    if(window.referenced.actor == actor)
                    {
                        Config.selectedUnit = actor;
                        ScrollWindow.moveAllToLeftAndRemove(true);
                        ScrollWindow.showWindow("inspect_unit");
                    }
                    else
                    {
                        window.window.scrollWindow.hide();
                        new inspect_family_window(actor.gameObject.GetComponent<HeraldicComponent>().Heraldic);
                    }
                }

                public avatarButton(Actor pActor, inspect_family_window pWindow, Vector2 pos, float size = 32) {
                    actor = pActor;
                    window = pWindow;
                    ActorData actor_data = Reflection.GetField(typeof(Actor), pActor, "data") as ActorData;
                    String button_uuid = Guid.NewGuid().ToString();

                    button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, null, pActor.getName(), "later", pos, ButtonType.Click, window.window.content.transform, inspect_family_window_button);
                    GameObject loader_object = new GameObject("loader");
                    //loader_object.transform.parent = window.content.transform; // <-- Debugging puroposes
                    UnitAvatarLoader loader = loader_object.AddComponent<UnitAvatarLoader>() as UnitAvatarLoader;
                    loader.load(pActor);

                    Image avatar = loader.transform.GetChild(0).gameObject.GetComponent<Image>();

                    button.gameObject.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = avatar.sprite;

                    button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size, size, 0);

                    /*
                    Image button_background = button.GetComponent<Image>();
                    button_background.enabled = false;
                    */
                }
            }

            public inspect_family_window(HeraldicInfo pInfo)
            {
                referenced = pInfo;

                // Yes you are right! this will create multiple windows that are not going to be deleted
                // this should be fixed but its ok for now since i dont think someone will open like 10000 windows.

                String window_uuid = Guid.NewGuid().ToString();
                window = new WindowLibrary.EasyScrollWindow("inspect_family_window_" + window_uuid, "Family Inspector");

                avatarButton portrait = new avatarButton(pInfo.actor, this, new Vector2(100, -30), 45);

                if(pInfo.father != null && pInfo.father.actor != null)
                {
                    new avatarButton(pInfo.father.actor, this, new Vector2(80, -80));
                }

                if (pInfo.mother != null && pInfo.mother.actor != null)
                {
                    new avatarButton(pInfo.mother.actor, this, new Vector2(115, -80));
                }

                window.UpdateVerticalRect((float)220);

                float lastX = 40;
                float lastY = -150;
                float lastVerticalRectMinus = -220;
                float lastVerticalRect = 220;

                foreach (HeraldicInfo child in pInfo.childs)
                {
                    new avatarButton(child.actor, this, new Vector2(lastX, lastY));
                    if(lastX < 145)
                    {
                        lastX += 35;
                    }
                    else
                    {
                        lastX = 40;
                        lastY += -35;
                    }

                    if(lastY == lastVerticalRectMinus)
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
            }
        }
    }
}
