using Mojai.Libraries.Other;
using UnityEngine;
using ReflectionUtility;

namespace HeraldicBox
{
    class HeraldicBoxActions
    {
        // ====================================================
        // HERALDICBOXACTIONS: In charge of the functionality.
        // ====================================================
        public static void Setup()
        {
            // ====================================================
            // DROPS SECTION
            // ====================================================

            mDropsLibrary.customDropPower inspectfamily_drop = new mDropsLibrary.customDropPower(
                "heraldic_inspectfamily_drop",
                new PowerAction(heraldic_inspectfamily_drop_action)
            );

            mDropsLibrary.customDropPower newfamily_drop = new mDropsLibrary.customDropPower(
                "heraldic_newfamily_drop",
                new PowerAction(heraldic_newfamily_drop_action)
            );

            //  SUB-SECTION TOOLS:
            mDropsLibrary.spawnDropPower tool_deleteall_families = new mDropsLibrary.spawnDropPower(
                "tool_deleteall_families",
                new DropsAction(heraldic_tool_deleteallfamilies)
            );
        }

        // ====================================================
        // ACTIONS SECTION
        // ====================================================

        public static bool heraldic_newfamily_drop_action(WorldTile pTile = null, GodPower pPower = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            if(MapBox.instance.temp_map_objects.Count > 0)
            {
                Actor actor = (Actor)MapBox.instance.temp_map_objects[0];
                ActorData actor_data = Reflection.GetField(typeof(Actor), actor, "data") as ActorData;
                if (actor.GetComponent<HeraldicComponent>() == null)
                {
                    HeraldicComponent component = actor.gameObject.AddComponent<HeraldicComponent>();
                    component.Heraldic = new HeraldicInfo(actor_data, actor);
                    new Family(component.Heraldic);
                    WorldTip.instance.showToolbarText(actor.getName() + " Lineage starts here");
                    component.Heraldic.TryUpdateActorInfo();
                    CornerAye.instance.startAye(); // <-- Huh?
                }
                else
                {
                    WorldTip.instance.showToolbarText(actor.getName() + " Already have a fmily");
                }
            }
            return true;
        }

        public static bool heraldic_inspectfamily_drop_action(WorldTile pTile = null, GodPower pPower = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            for (int i = 0; i < MapBox.instance.temp_map_objects.Count; i++)
            {
                Actor actor = (Actor)MapBox.instance.temp_map_objects[i];
                HeraldicComponent component = actor.gameObject.GetComponent<HeraldicComponent>();
                if (component != null)
                {
                    new HeraldicBoxUI.inspect_family_window(component.Heraldic);
                    break;
                }
            }
            return true;
        }

        public static void heraldic_tool_deleteallfamilies(WorldTile pTile = null, string pDropID = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            for (int i = 0; i < MapBox.instance.temp_map_objects.Count; i++)
            {
                Actor actor = (Actor)MapBox.instance.temp_map_objects[i];
                HeraldicComponent component = actor.gameObject.GetComponent<HeraldicComponent>();
                if (component == null)
                {
                    actor.killHimself();
                }
            }
        }

        // SUB-SECTION: UI ACTIONS

        public static void aboutme_mojai()
        {
            Application.OpenURL("https://github.com/mojaidev");
        }

        public static void show_index()
        {
            new HeraldicBoxUI.family_index_window();
        }

        public static void show_settings()
        {
            HeraldicBoxUI.settings_window.show();
        }
    }
}
