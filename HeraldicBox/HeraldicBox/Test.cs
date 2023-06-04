using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mojai.Libraries.UI;
using System.Threading;
using UnityEngine;
using NCMS.Utils;
using ReflectionUtility;

namespace HeraldicBox
{
    class Test
    {
        public static void debug_1(WorldTile pTile = null, string pDropID = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            for (int i = 0; i < MapBox.instance.temp_map_objects.Count; i++)
            {
                Actor actor = (Actor)MapBox.instance.temp_map_objects[i];
                ActorData actor_data = Reflection.GetField(typeof(Actor), actor, "data") as ActorData;
                HeraldicComponent component = actor.gameObject.AddComponent<HeraldicComponent>();
                component.Heraldic = new HeraldicInfo(actor_data, actor);
                Family family = new Family(component.Heraldic);
                WorldTip.showNowTop("Family Created");
            }
        }

        public static void debug_2(WorldTile pTile = null, string pDropID = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            Actor actor = (Actor)MapBox.instance.temp_map_objects[0];
            HeraldicComponent component = actor.gameObject.GetComponent<HeraldicComponent>();
            if (component != null)
            {
                new HeraldicBoxUI.inspect_family_window(component.Heraldic);
            }
        }

        public static void Init()
        {
            Patches.Setup();
            TabLibrary.Tab debugTab = new TabLibrary.Tab("tab_debug_heraldic", "HeraldicBox [DEBUG TAB]", Resources.Load<Sprite>("ui/icons/new_icon"), new Vector2(1f, 1f), new Vector2(150, 49.62f));
            Mojai.Libraries.Other.DropsLibrary.Drop drop1 = new Mojai.Libraries.Other.DropsLibrary.Drop("debugdrop1", new DropsAction(debug_1), "blessing");
            Mojai.Libraries.Other.DropsLibrary.Drop drop2 = new Mojai.Libraries.Other.DropsLibrary.Drop("debugdrop2", new DropsAction(debug_2), "blessing");
            PowerButton debugButton1 = PowerButtons.CreateButton("debugdrop1", Resources.Load<Sprite>("ui/icons/new_icon"), "Debug Button", "Debug Button", Vector2.zero, ButtonType.GodPower);
            PowerButton debugButton2 = PowerButtons.CreateButton("debugdrop2", Resources.Load<Sprite>("ui/icons/new_icon"), "Debug Button 2", "Debug Button", Vector2.zero, ButtonType.GodPower);
            TabLibrary.Tab.AddButtonToTab(debugButton1, "tab_debug_heraldic", new Vector2(211.2f, 18));
            TabLibrary.Tab.AddButtonToTab(debugButton2, "tab_debug_heraldic", new Vector2(311.2f, 18));
        }
    }
}