using Newtonsoft.Json;
using ReflectionUtility;
using System.Collections.Generic;

namespace HeraldicBox
{
    class Saves
    {
        public static string jsonFamilies;
        public static string jsonInfos;

        public static bool LoadHeraldic()
        {
            List<Family> families = JsonConvert.DeserializeObject<List<Family>>(jsonFamilies);
            List<HeraldicInfo> infos = JsonConvert.DeserializeObject<List<HeraldicInfo>>(jsonInfos);

            foreach(Family pFamily in families)
            {
                Family.families.Pop();
                families.Add(pFamily);
            }

            foreach (Actor pActor in World.world.units)
            {
                ActorData actor_data = Reflection.GetField(typeof(Actor), pActor, "data") as ActorData;

                foreach (HeraldicInfo pInfo in infos)
                {
                    if(pInfo.actorData.id == actor_data.id)
                    {
                        HeraldicComponent component = pActor.gameObject.AddComponent<HeraldicComponent>();
                        component.Heraldic = pInfo;
                        pInfo.actor = pActor;
                    }
                }
            }
            return true;
        }

        public static bool SaveHerladic()
        {
            jsonInfos = JsonConvert.SerializeObject(HeraldicInfo.allInfo);
            Mojai.Mod.Util.Print(jsonInfos);
            jsonFamilies = JsonConvert.SerializeObject(Family.families);
            return true;
        }
    }
}
