using Mojai.Libraries.UI;
using UnityEngine;
using NCMS.Utils;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using ReflectionUtility;

namespace HeraldicBox
{
    // ====================================================
    // HERALDICBOXUI: In charge of drawing the ui.
    //
    // DISCLAIMER: This code is very bad, please understand
    // im lazy.
    // ====================================================
    class HeraldicBoxUI
    {
        public static void SetupAll()
        {
            tab_setup();
            inspect_unit_changes();
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

            TabLibrary.Tab tab = new TabLibrary.Tab("tab_heraldicbox", "HeraldicBox", Resources.Load<Sprite>("ui/icons/tab_heraldic"), new Vector2(1f, 1f), new Vector2(Convert.ToSingle(HeraldicBoxSettings.GetSetting("tab_HeraldicBox_Position")), 49.62f));

            PowerButton newfamily_button = PowerButtons.CreateButton("heraldic_newfamily_drop", Resources.Load<Sprite>("ui/icons/new_family_icon"), "New Family", "Create a new famliy by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton tool_deleteall_families_button = PowerButtons.CreateButton("tool_deleteall_families", Resources.Load<Sprite>("ui/icons/deleteall_icon"), "Delete Orphans", "Delete all units without a family.", Vector2.zero, ButtonType.GodPower);
            PowerButton inspectfamily_drop_button = PowerButtons.CreateButton("heraldic_inspectfamily_drop", Resources.Load<Sprite>("ui/icons/inspect_icon"), "Inspect Family", "Get a unit's family window by dropping this.", Vector2.zero, ButtonType.GodPower);
            PowerButton family_index_button = PowerButtons.CreateButton("family_index_button", Resources.Load<Sprite>("ui/icons/index_icon"), "Family Index", "Track down families.", Vector2.zero, ButtonType.Click, null, HeraldicBoxActions.show_index);
            PowerButton aboutme_button = PowerButtons.CreateButton("aboutme_button_mojai", Resources.Load<Sprite>("ui/icons/mojai_author_icon"), "About Me", "Ñ", Vector2.zero);
            PowerButton settings_button = PowerButtons.CreateButton("heraldic_settings", Resources.Load<Sprite>("ui/icons/options_icon"), "Settings", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero, ButtonType.Click, null, HeraldicBoxActions.show_settings);
            PowerButton save_button = PowerButtons.CreateButton("heraldic_save_button", Resources.Load<Sprite>("ui/icons/save_icon"), "Save", "Modify HeraldicBox behaviour by changing the settings.", Vector2.zero, ButtonType.Click, null, HeraldicBoxActions.show_settings);

            TabLibrary.Tab.AddButtonToTab(newfamily_button,                         "tab_heraldicbox", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(tool_deleteall_families_button,           "tab_heraldicbox", new Vector2(254.2f, 18));
            TabLibrary.Tab.AddButtonToTab(inspectfamily_drop_button,                "tab_heraldicbox", new Vector2(297.2f, 18));
            TabLibrary.Tab.AddButtonToTab(family_index_button,                      "tab_heraldicbox", new Vector2(340.2f, 18));
            TabLibrary.Tab.AddButtonToTab(settings_button,                          "tab_heraldicbox", new Vector2(383.2f, 18));
            TabLibrary.Tab.AddButtonToTab(save_button,                              "tab_heraldicbox", new Vector2(426.2f, 18));
            TabLibrary.Tab.AddButtonToTab(aboutme_button,                           "tab_heraldicbox", new Vector2(803.2f, 18));
        }

        private static void inspect_unit_changes()
        {
            // ====================================================
            // INSPECT UNIT UI CHANGES
            // ====================================================

            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            ScrollWindow inspect_unit_window = ScrollWindow.get("inspect_unit");
            inspect_unit_window.gameObject.SetActive(false);

            GameObject background = inspect_unit_window.transform.Find("Background").gameObject;
            PowerButton button = PowerButtons.CreateButton("inspect_unit_inspect_family_lol", Resources.Load<Sprite>("ui/icons/inspect_icon"), "Family", "", new Vector2(116.8f, -35.5f), ButtonType.Click, background.transform, HeraldicBoxActions.inspect_family_button_inside);
            button.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/backgroundTabButton");
        }

        private class HeraldicAvatarButton
        {
            public PowerButton button;
            private HeraldicInfo info;

            public HeraldicAvatarButton(HeraldicInfo pInfo, GameObject parent, Vector2 pos, int importance = 0, float size = 32, UnityEngine.Events.UnityAction<HeraldicInfo> pAction = null)
            {
                void doAction()
                {
                    pAction(pInfo);
                }

                info = pInfo;
                String button_uuid = Guid.NewGuid().ToString();
                info.TryUpdateActorInfo();

                if (pInfo.actor != null)
                {
                    button = PowerButtons.CreateButton("heraldic_avatar_button_" + button_uuid, info.savedSprite, info.actorName, "Living | " + info.city, pos, ButtonType.Click, parent.transform, doAction);
                }
                else
                {
                    button = PowerButtons.CreateButton("inspect_family_window_button_" + button_uuid, info.savedSprite, pInfo.actorName, "Dead", pos, ButtonType.Click, parent.transform, doAction);
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

        public class settings_window
        {
            // ====================================================
            // Settings Windows below
            // ====================================================

            private static WindowLibrary.EasyScrollWindow window;

            private static PowerButton lgbt_button;
            private static PowerButton asexual_button;

            public static void show()
            {
                if (window == null)
                {
                    window = new WindowLibrary.EasyScrollWindow("setting_window_heraldicbox", "Settings");
                    window.scrollWindow.show();

                    // BOOLEAN SETTINGS:
                    float lastX = 40;
                    float lastY = -40;
                    foreach (var setting in HeraldicBoxSettings.instance.settings)
                    {
                        if(setting.Value.obj is bool)
                        {
                            HeraldicBoxSettings.Setting daSetting = setting.Value;
                            String button_uuid = Guid.NewGuid().ToString();
                            PowerButton button = PowerButtons.CreateButton("setting_" + setting.Key, Resources.Load<Sprite>("ui/icons/options_icon"), daSetting.configName, "", new Vector2(lastX, lastY), ButtonType.Toggle, window.content.transform, daSetting.switchBoolConfig);
                            if ((bool)daSetting.obj == true)
                            {
                                PowerButtons.ToggleButton(button.name);
                            }
                            lastX += 40;
                            if (lastX >= 170)
                            {
                                lastX = 40;
                                lastY += -40;
                            }
                        }
                    }

                    lastY += -60;
                    WindowLibrary.EasyInner string_settings_inner = new WindowLibrary.EasyInner("heraldicbox_settings_inner1", window.content.transform, new Vector3(180, 70), new Vector3(100, lastY));

                    // STRING (INPUT) SETTINGS:
                    lastY = 0;
                    foreach (var setting in HeraldicBoxSettings.instance.settings)
                    {
                        if (setting.Value.obj is string)
                        {
                            WindowLibrary.AddTextToObject(string_settings_inner.inner, setting.Value.configName, 6, new Vector3(0, lastY + 15)).AddComponent<Shadow>();
                            void ActionInput(string input)
                            {
                                HeraldicBoxSettings.ExecuteCustomInputSetting(setting.Key, input);
                            }
                            WindowLibrary.EasyInputField inputField = new WindowLibrary.EasyInputField(string_settings_inner.inner.transform, (string)setting.Value.obj, Vector3.zero, ActionInput);
                            lastY += -20; // (-20) isnt the real value, its a placeholder.
                        }
                    }


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

            private Family referenced;
            private WindowLibrary.EasyScrollWindow window;

            private void Delete_Family()
            {
                referenced.Destroy();
                window.scrollWindow.clickHide();
                ScrollWindow.moveAllToLeftAndRemove(true);
                new family_index_window();
            }

            private void ApplyText_familyName(string pInput)
            {
                referenced.familyName = pInput;
            }

            private void ApplyText_lastName(string pInput)
            {
                referenced.lastName = pInput;
            }

            private void inspect_founder(HeraldicInfo info)
            {
                window.scrollWindow.clickHide();
                new inspect_family_window(info);
            }

            public edit_family_window(Family pFamily)
            {
                referenced = pFamily;
                window = new WindowLibrary.EasyScrollWindow("edit_family_window_heraldicbox", "Edit Family");
                window.Clear();

                new WindowLibrary.EasyInputField(window.content.transform, pFamily.familyName, new Vector3(130, -20), ApplyText_familyName);
                new WindowLibrary.EasyInputField(window.content.transform, pFamily.lastName, new Vector3(130, -40), ApplyText_lastName);

                new HeraldicAvatarButton(pFamily.members[0], window.content, new Vector2(95, -70), 0, 32, inspect_founder);

                new WindowLibrary.EasyRedButton(window.content.transform, "DELETE", new Vector3(50, -200), new Vector2(80, 20), Delete_Family);

                GameObject NameInput = WindowLibrary.AddTextToObject(window.content, "Name:", 10, new Vector3(30, -20), window.scrollWindow.name);
                GameObject lastNameInput = WindowLibrary.AddTextToObject(window.content, "Last Name:", 10, new Vector3(41, -40), window.scrollWindow.name);
                GameObject FounderText = WindowLibrary.AddTextToObject(window.content, "Founder:", 10, new Vector3(35.20f, -60), window.scrollWindow.name);
                NameInput.AddComponent<Shadow>();
                lastNameInput.AddComponent<Shadow>();
                FounderText.AddComponent<Shadow>();
                // The colors right down are: #FFBC66FF
                FounderText.GetComponent<Text>().color = new Color(1f, 0.7372549f, 0.4f, 1f);
                NameInput.GetComponent<Text>().color = new Color(1f, 0.7372549f, 0.4f, 1f);
                lastNameInput.GetComponent<Text>().color = new Color(1f, 0.7372549f, 0.4f, 1f);

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
                void buttonClick()
                {
                    window.scrollWindow.hide();
                    new edit_family_window(pFamily);
                }

                String button_uuid = Guid.NewGuid().ToString();
                HeraldicInfo founder = pFamily.members[0];
                Sprite buttonSprite = founder.savedSprite;

                if(pFamily.aliveMembers.Count <= 0)
                {
                    buttonSprite = Resources.Load<Sprite>("ui/icons/dead");
                }

                // Should be changed
                PowerButton button = PowerButtons.CreateButton("familyindex_look_button_" + button_uuid, buttonSprite, pFamily.familyName, "", position, ButtonType.Click, window.content.transform, buttonClick);
                button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 38);
                button.gameObject.GetComponent<Image>().sprite = pFamily.shield;
                //button.gameObject.GetComponent<Image>().color = pFamily.familyColor;
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
                    FamilyButton(pFamily, new Vector2(lastX, lastY));

                    lastX += 40;
                    if (lastX >= 160)
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
            // WARNING: BAD CODE BELOW!!
            // ====================================================

            // Ill make a method to create Inners someday but that is in the future.

            private static List<inspect_family_window> openedWindows = new List<inspect_family_window>();
            private WindowLibrary.EasyScrollWindow window;
            private HeraldicInfo referenced;

            public static void clickHide_Postfix(ScrollWindow __instance)
            {
                foreach(inspect_family_window pWindow in openedWindows)
                {
                    UnityEngine.Object.Destroy(pWindow.window.scrollWindow.gameObject);
                }

                openedWindows = new List<inspect_family_window>();
            }

            // inspect_family_window_button() is an exception that is not in HeraldicBoxActions
            private void inspect_family_window_button(HeraldicInfo info)
            {
                // scrollWindow.hide() is better
                if (referenced == info)
                {
                    if (referenced.actor != null)
                    {
                        window.scrollWindow.clickHide();
                        Config.selectedUnit = info.actor;
                        ScrollWindow.moveAllToLeftAndRemove(true);
                        ScrollWindow.showWindow("inspect_unit");
                    }
                }
                else
                {
                    window.scrollWindow.hide();
                    new inspect_family_window(info);
                }
            }

            public inspect_family_window(HeraldicInfo pInfo)
            {
                openedWindows.Add(this);
                referenced = pInfo;
                pInfo.TryUpdateActorInfo();
                String window_uuid = Guid.NewGuid().ToString();

                window = new WindowLibrary.EasyScrollWindow("inspect_family_window_" + window_uuid, pInfo.actorName);
                window.scrollWindow.gameObject.transform.Find("Background/Title").GetComponent<Text>().text = pInfo.actorName;
                window.UpdateVerticalRect((float)220);

                HeraldicAvatarButton portrait = new HeraldicAvatarButton(pInfo, window.content, new Vector2(40, -30), 1, 45, inspect_family_window_button);

                GameObject parents_inner = new WindowLibrary.EasyInner("parents_inner", window.content.transform, new Vector3(170, 40), new Vector3(105, -100, 0)).inner;
                if (pInfo.father != null)
                {
                    GameObject father_button = new HeraldicAvatarButton(pInfo.father, parents_inner, new Vector2(-60, 0), 0, 32, inspect_family_window_button).button.gameObject;
                }
                if (pInfo.mother != null)
                {
                    GameObject mother_button = new HeraldicAvatarButton(pInfo.mother, parents_inner, new Vector2(-25, 0), 0, 32, inspect_family_window_button).button.gameObject;
                }

                GameObject children_inner = new WindowLibrary.EasyInner("children_inner", window.content.transform, new Vector3(170, 40), new Vector3(105, -160, 0)).inner;
                float lastX = -60;
                float lastY = -160;
                foreach (HeraldicInfo child in pInfo.children)
                {
                    GameObject childButton = new HeraldicAvatarButton(child, children_inner, new Vector2(lastX, 0), 0, 32, inspect_family_window_button).button.gameObject;
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
