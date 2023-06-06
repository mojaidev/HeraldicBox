using Mojai.Libraries.Other;
using ReflectionUtility;

namespace HeraldicBox
{
    class HeraldicBoxActions
    {
        public static void Setup()
        {
            // ====================================================
            // DROPS SECTION
            // ====================================================

            mDropsLibrary.customPower inspectfamily_drop = new mDropsLibrary.customPower(
                "heraldic_inspectfamily_drop",
                new PowerAction(heraldic_inspectfamily_drop_action)
            );

            mDropsLibrary.customPower newfamily_drop = new mDropsLibrary.customPower(
                "heraldic_newfamily_drop",
                new PowerAction(heraldic_newfamily_drop_action)
            );
        }

        // ====================================================
        // ACTIONS SECTION
        // ====================================================

        public static bool heraldic_newfamily_drop_action(WorldTile pTile = null, GodPower pPower = null)
        {
            MapBox.instance.getObjectsInChunks(pTile, 1, MapObjectType.Actor);
            for (int i = 0; i < MapBox.instance.temp_map_objects.Count; i++)
            {
                Actor actor = (Actor)MapBox.instance.temp_map_objects[i];
                ActorData actor_data = Reflection.GetField(typeof(Actor), actor, "data") as ActorData;
                HeraldicComponent component = actor.gameObject.AddComponent<HeraldicComponent>();
                component.Heraldic = new HeraldicInfo(actor_data, actor);
                new Family(component.Heraldic);
                WorldTip.instance.showToolbarText(actor.getName() + " Created a beautiful family");
                component.Heraldic.actorName = actor.getName();
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
                }
            }
            return true;
        }
    }
}
