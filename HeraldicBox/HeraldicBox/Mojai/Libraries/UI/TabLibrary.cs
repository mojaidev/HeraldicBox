using System;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;
using NCMS;

namespace Mojai.Libraries.UI
{
    class TabLibrary
    {
        /*
         * Stole this code from....
         * i dont remember
         */
        public class Tab
        {
            private string Tab_Name, Name;
            private Sprite SpriteForTab;
            private Vector2 Scale, Position;

            public void init()
            {
                GameObject OtherTabButton = GameObjects.FindEvenInactive("Button_Other");
                if (OtherTabButton != null)
                {
                    Localization.AddOrSet("Button " + Tab_Name, Name);
                    Localization.AddOrSet($"{"Button " + Tab_Name} Description", "");
                    Localization.AddOrSet("", "");
                    Localization.AddOrSet(Tab_Name, Name);

                    GameObject newTabButton = GameObject.Instantiate(OtherTabButton);
                    newTabButton.transform.SetParent(OtherTabButton.transform.parent);
                    Button buttonComponent = newTabButton.GetComponent<Button>();
                    TipButton tipButton = buttonComponent.gameObject.GetComponent<TipButton>();
                    tipButton.textOnClick = "Button " + Tab_Name;
                    tipButton.textOnClickDescription = $"{"Button " + Tab_Name} Description";
                    tipButton.text_description_2 = "";
                    newTabButton.transform.localPosition = Position;
                    newTabButton.transform.localScale = Scale;
                    newTabButton.name = "Button " + Tab_Name;
                    Sprite spriteForTab = SpriteForTab;
                    newTabButton.transform.Find("Icon").GetComponent<Image>().sprite = spriteForTab;

                    GameObject OtherTab = GameObjects.FindEvenInactive("Tab_Other");
                    foreach (Transform child in OtherTab.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                    GameObject additionalPowersTab = GameObject.Instantiate(OtherTab);
                    foreach (Transform child in additionalPowersTab.transform)
                    {
                        if (child.gameObject.name == "tabBackButton" || child.gameObject.name == "-space")
                        {
                            child.gameObject.SetActive(true);
                            continue;
                        }

                        GameObject.Destroy(child.gameObject);
                    }

                    foreach (Transform child in OtherTab.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    additionalPowersTab.transform.SetParent(OtherTab.transform.parent);
                    PowersTab powersTabComponent = additionalPowersTab.GetComponent<PowersTab>();
                    powersTabComponent.powerButton = buttonComponent;
                    powersTabComponent.powerButtons.Clear();
                    additionalPowersTab.name = Tab_Name;
                    powersTabComponent.powerButton.onClick = new Button.ButtonClickedEvent();
                    powersTabComponent.powerButton.onClick.AddListener(() => tabOnClick(Tab_Name));
                    Reflection.SetField<GameObject>(powersTabComponent, "parentObj", OtherTab.transform.parent.parent.gameObject);
                    additionalPowersTab.SetActive(true);
                    powersTabComponent.powerButton.gameObject.SetActive(true);
                }
            }
            public void tabOnClick(string tabID)
            {
                GameObject AdditionalTab = GameObjects.FindEvenInactive(tabID);
                PowersTab AdditionalPowersTab = AdditionalTab.GetComponent<PowersTab>();
                AdditionalPowersTab.showTab(AdditionalPowersTab.powerButton);
            }

            public static void AddButtonToTab(PowerButton button, string tab_name, Vector2 position)
            {
                GameObject gameObject = GameObjects.FindEvenInactive(tab_name);
                PowersTab tab = gameObject.GetComponent<PowersTab>();

                PowersTab powersTab = tab;
                button.transform.SetParent(powersTab.transform);
                button.transform.localPosition = position;
                button.transform.localScale = new Vector2(1f, 1f);
            }

            public Tab(String tab_id, String name, Sprite spritefortab, Vector2 scale, Vector2 position)
            {
                Tab_Name = tab_id;
                Name = name;
                SpriteForTab = spritefortab;
                Scale = scale;
                Position = position;
                this.init();
            }
        }
    }
}
