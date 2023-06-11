using Mojai.Libraries.UI;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine.UI;
using System;

namespace HeraldicBox
{
    // ====================================================
    // HERALDICBOXUI
    // DISCLAIMER: This code is very bad, please understand
    // im lazy.
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

            TabLibrary.Tab tab = new TabLibrary.Tab("tab_heraldicbox", "HeraldicBox", Resources.Load<Sprite>("ui/icons/tab_heraldic"), new Vector2(1f, 1f), new Vector2(-400, 49.62f));
            PowerButton newfamily_button = PowerButtons.CreateButton("heraldic_newfamily_drop", Resources.Load<Sprite>("ui/icons/new_family_icon"), "New Family", "Create a new famliy by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton inspectfamily_drop_button = PowerButtons.CreateButton("heraldic_inspectfamily_drop", Resources.Load<Sprite>("ui/icons/inspect_icon"), "Inspect Family", "Get a unit's family tree by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton family_index_button = PowerButtons.CreateButton("family_index_button", Resources.Load<Sprite>("ui/icons/index_icon"), "Family Index", "Track down families [BETA].", Vector2.zero, ButtonType.Click, null, HeraldicBoxActions.show_index);
            PowerButton aboutme_button = PowerButtons.CreateButton("aboutme_button_mojai", Resources.Load<Sprite>("ui/icons/mojai_author_icon"), "About Me", "Ñ", Vector2.zero);
            PowerButton settings_button = PowerButtons.CreateButton("heraldic_settings", Resources.Load<Sprite>("ui/icons/options_icon"), "Settings", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero, ButtonType.Click, null, HeraldicBoxActions.show_settings);

            TabLibrary.Tab.AddButtonToTab(newfamily_button,                         "tab_heraldicbox", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(inspectfamily_drop_button,                "tab_heraldicbox", new Vector2(254.2f, 18));
            TabLibrary.Tab.AddButtonToTab(family_index_button,                      "tab_heraldicbox", new Vector2(297.2f, 18));
            TabLibrary.Tab.AddButtonToTab(settings_button,                          "tab_heraldicbox", new Vector2(340.2f, 18));
            TabLibrary.Tab.AddButtonToTab(aboutme_button,                           "tab_heraldicbox", new Vector2(803.2f, 18));
        }

        public class settings_window
        {
            // ====================================================
            // Settings Windows below
            // ====================================================

            private static WindowLibrary.EasyScrollWindow window;

            public static void show()
            {
                if (window == null)
                {
                    window = new WindowLibrary.EasyScrollWindow("setting_window_heraldicbox", "Settings");
                    window.scrollWindow.show();
                    PowerButtons.CreateButton("changesetting_lgbt_button", Resources.Load<Sprite>("ui/icons/options_icon"), "LGBT Reproduction", "", Vector2.zero, ButtonType.Toggle, window.content.transform);
                }
                else
                {
                    window.scrollWindow.show();
                }
            }
        }

        public class edit_family_window
        {
            // ====================================================
            // Family Index
            // ====================================================

            private WindowLibrary.EasyScrollWindow window;

            public edit_family_window()
            {
                window = new WindowLibrary.EasyScrollWindow("edit_family_window_heraldicbox", "Edit Family");
                window.Clear();



                window.scrollWindow.show();
            }

        }

        public class family_index_window
        {
            // ====================================================
            // Family Index
            // ====================================================

            private WindowLibrary.EasyScrollWindow window;

            private PowerButton FamilyButton(Family pFamily, Vector2 position)
            {
                String button_uuid = Guid.NewGuid().ToString();
                HeraldicInfo founder = pFamily.members[0];
                PowerButton button = PowerButtons.CreateButton("familyindex_look_button_" + button_uuid, founder.savedSprite, pFamily.familyName, "", position, ButtonType.Click, window.content.transform);
                button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 38);
                button.gameObject.GetComponent<Image>().sprite = pFamily.shield;
                button.gameObject.GetComponent<Image>().color = pFamily.familyColor;
                return button;
            }

            public family_index_window()
            {
                window = new WindowLibrary.EasyScrollWindow("family_index_window_heraldicbox", "Index");
                window.Clear();

                float lastX = 40;
                float lastY = -30;
                float scrollLimit = -190;
                float scrollSize = 207;
                foreach (Family pFamily in Family.families)
                {
                    if(lastX <= 160)
                    {
                        FamilyButton(pFamily, new Vector2(lastX, lastY));
                        lastX += 40;
                    }
                    else
                    {
                        lastX = 40;
                        lastY += -40;

                        if(lastY <= scrollLimit)
                        {
                            scrollLimit += -40;
                            scrollSize += 50;
                            window.UpdateVerticalRect(scrollSize);
                        }
                    }
                }

                window.scrollWindow.show();
            }

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

            // Ill make a method to create Inners someday but that is in the future.

            private WindowLibrary.EasyScrollWindow window;
            private HeraldicInfo referenced;

            private class avatarButton
            {
                public PowerButton button;
                private inspect_family_window window;
                private HeraldicInfo info;

                void inspect_family_window_button()
                {
                    // scrollWindow.hide() is better but ill use the postfixed one by now
                    if (window.referenced == info)
                    {
                        if (window.referenced.actor != null)
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

                public avatarButton(HeraldicInfo pInfo, GameObject parent, inspect_family_window pWindow, Vector2 pos, int importance = 0, float size = 32) {
                    info = pInfo;
                    window = pWindow;
                    String button_uuid = Guid.NewGuid().ToString();
                    info.TryUpdateActorInfo();

                    if (pInfo.actor != null)
                    {
                        button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, info.savedSprite, info.actorName, "Living | " + info.city, pos, ButtonType.Click, parent.transform, inspect_family_window_button);
                    }
                    else
                    {
                        Mojai.Mod.Util.Print(info.actorName + " THE FUCK!!!???? 1");
                        button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, info.savedSprite, pInfo.actorName, "Dead", pos, ButtonType.Click, parent.transform, inspect_family_window_button);
                        button.gameObject.transform.Find("Icon").gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f); // <-- This color is gray: 808080FF
                    }

                    if (importance == 1)
                    {
                        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/importance_1");
                        
                    }
                    /*
                    if(importance == 2)
                    {
                        // this looks so fucking cool...
                        button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/importance_1");
                        GameObject goldRing = new GameObject("gold_ring_FUCKYEAH_THIS_LOOKS_AWESOME");
                        goldRing.AddComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/importance_2");
                        goldRing.transform.parent = button.gameObject.transform;
                    }
                    */
                    button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector3(size, size, 0);
                }
            }

            public inspect_family_window(HeraldicInfo pInfo)
            {
                referenced = pInfo;
                pInfo.TryUpdateActorInfo();

                window = new WindowLibrary.EasyScrollWindow("inspect_family_window", pInfo.actorName);
                window.Clear();
                window.UpdateVerticalRect((float)220);

                avatarButton portrait = new avatarButton(pInfo, window.content, this, new Vector2(40, -30), 1, 45);

                GameObject parents_inner = new WindowLibrary.EasyInner("parents_inner", window.content.transform, new Vector3(170, 40), new Vector3(105, -100, 0)).inner;

                if (pInfo.father != null)
                {
                    GameObject father_button = new avatarButton(pInfo.father, parents_inner, this, new Vector2(-60, 0)).button.gameObject;
                }

                if (pInfo.mother != null)
                {
                    GameObject mother_button = new avatarButton(pInfo.mother, parents_inner, this, new Vector2(-25, 0)).button.gameObject;
                }

                GameObject children_inner = new WindowLibrary.EasyInner("children_inner", window.content.transform, new Vector3(170, 40), new Vector3(105, -160, 0)).inner;
                float lastX = -60;
                float lastY = -160;
                foreach (HeraldicInfo child in pInfo.children)
                {
                    GameObject childButton = new avatarButton(child, children_inner, this, new Vector2(lastX, 0)).button.gameObject;
                    if (lastX < 45)
                    {
                        lastX += 35;
                    }
                    else
                    {
                        lastX = -60;

                        children_inner = new WindowLibrary.EasyInner("children_inner", window.content.transform, new Vector3(170, 40), new Vector3(105, lastY + -45, 0)).inner;
                        lastY += -45;
                        window.UpdateVerticalRect(window.content.GetComponent<RectTransform>().sizeDelta.y + 50);
                    }

                }

                // I dont know why but when i put the text before UpdateVerticalRect the text x size is updated to 90, WTF?
                // UPDATE: it happens with all the objects idk why
                // ANOTHER UPDATE: turns out that RectTransform.anchorMin/anchorMax are the ones responsable.


                // fuck this.
                /*
                GameObject info_inner = new GameObject("info_inner");
                info_inner.AddComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/windowInnerSliced");
                info_inner.transform.parent = window.content.transform;
                info_inner.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                info_inner.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                info_inner.transform.localScale = new Vector3(1, 1, 1);
                info_inner.transform.localPosition = new Vector3(130, -30, 0);
                info_inner.GetComponent<RectTransform>().sizeDelta = new Vector3(125, 40);

                string textInfo = $"Kingdom: {pInfo.kingdom}\nCity: {pInfo.city}\n";
                GameObject textInfoObj = WindowLibrary.AddTextToObject(window.content, textInfo, 7, new Vector3(0, 0, 0), window.scrollWindow.name);
                textInfoObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                textInfoObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                textInfoObj.transform.parent = info_inner.transform;
                textInfoObj.transform.localPosition = new Vector3(-15, 0, 0);
                textInfoObj.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
                //textInfoObj.AddComponent<Shadow>();
                */


                // Why i dont do this with a single string? well because it will bug and overlap with parents_inner, i spent like 3 hours on catching that. fuck
                GameObject title = window.scrollWindow.gameObject.transform.Find("Background/Title").gameObject;
                GameObject textStat1 = WindowLibrary.AddTextToObject(window.content, "Parents: ", 10, new Vector3(40, -70, 0), window.scrollWindow.name);
                GameObject textStat2 = WindowLibrary.AddTextToObject(window.content, "Children: ", 10, new Vector3(40, -130, 0), window.scrollWindow.name);
                textStat1.AddComponent<Shadow>();
                textStat2.AddComponent<Shadow>();
                // The colors right down are: #FFBC66FF
                textStat1.GetComponent<Text>().color = new Color(1f, 0.7372549f, 0.4f, 1f);
                textStat2.GetComponent<Text>().color = new Color(1f, 0.7372549f, 0.4f, 1f);
                title.GetComponent<Text>().color = new Color(0.5882353f, 1f, 1f, 1f); // <-- Except this one, this is: 96FFFFFF

                window.scrollWindow.show();
            }
        }
    }
}
