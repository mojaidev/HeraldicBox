using Newtonsoft.Json;
using ReflectionUtility;
using System.Collections.Generic;

namespace HeraldicBox
{
    class Saves
    {
        public static string jsonFamilies;

        public static Results LoadHeraldic()
        {
            Results results = new Results();
            List<Family> families = JsonConvert.DeserializeObject<List<Family>>(jsonFamilies);
            List<Actor> loadedUnits = new List<Actor>();

            foreach(Family pFamily in families)
            {
                results.loaded_families += 1;
                Family.families.Add(pFamily);

                foreach(HeraldicInfo pInfo in pFamily.aliveMembers)
                {
                    bool loaded = false;
                    foreach (Actor pActor in World.world.units)
                    {
                        ActorData actor_data = Reflection.GetField(typeof(Actor), pActor, "data") as ActorData;

                        if(actor_data.id != pInfo.actorData.id)
                        {
                            continue;
                        }

                        HeraldicComponent component;

                        if (pActor.gameObject.GetComponent<HeraldicComponent>() != null)
                        {
                            component = pActor.gameObject.GetComponent<HeraldicComponent>();
                        }
                        else
                        {
                            component = pActor.gameObject.AddComponent<HeraldicComponent>();
                        }
                        
                        component.Heraldic = pInfo;
                        pInfo.actor = pActor;
                        loadedUnits.Add(pActor);
                        pInfo.TryUpdateActorInfo();
                        loaded = true;
                    }

                    if (loaded)
                    {
                        results.loaded_units += 1;
                    }
                    else
                    {
                        results.lost_units += 1;
                    }
                }
            }

            foreach(Actor pActor in loadedUnits)
            {
                HeraldicInfo info = pActor.gameObject.GetComponent<HeraldicComponent>().Heraldic;
                
                if(info.father != null)
                {
                    info.father.children.Add(info);
                    pActor.gameObject.GetComponent<HeraldicComponent>().Heraldic = info.father;
                }

                if(info.mother != null)
                {
                    info.mother.children.Add(info);
                }
            }

            return results;
        }

        public static bool SaveHerladic()
        {
            jsonFamilies = JsonConvert.SerializeObject(Family.families);
            return true;
        }

        public class Results
        {
            public int loaded_units;
            public int loaded_families;
            public int lost_units;
        }
    }
}
