using ReflectionUtility;
using System.Collections.Generic;
using System;

namespace HeraldicBox
{ 
    class Family
    {
        // ====================================================
        // PATCHES SECTION
        // ====================================================

        public static bool getName_Prefix(ActorBase __instance, ref string __result)
        {
            ActorData data = Reflection.GetField(typeof(ActorBase), __instance, "data") as ActorData;
            if (string.IsNullOrEmpty(data.name))
            {
                HeraldicComponent component = __instance.GetComponent<HeraldicComponent>();
                if (component != null)
                {
                    HeraldicInfo info = component.Heraldic;
                    if (info.family != null)
                    {
                        Race instanceRace = Reflection.GetField(typeof(ActorBase), __instance, "race") as Race;
                        data.generateName(__instance.asset, instanceRace);
                        data.name = data.name + " " + info.family.lastName;

                        if(info.mother != null)
                        {
                            if (info.mother.family != null)
                            {
                                // Ill fix incest later...
                                if(info.mother.family.lastName != info.family.lastName)
                                {
                                    data.name = data.name + " " + info.mother.family.lastName;
                                }
                            }
                        }

                        __result = data.name;
                        return false;
                    }
                }
            }
            return true;
        }

        // ====================================================
        // FAMILY SECTION
        // ====================================================

        public static List<Family> families = new List<Family>();

        [NonSerialized] public List<HeraldicInfo> members = new List<HeraldicInfo>();
        public string familyName, lastName;

        public string publicID; // <-- This is for the save system.

        public Family(HeraldicInfo pFounder)
        {
            if(pFounder != null)
            {
                publicID = Guid.NewGuid().ToString();
                familyName = "The " + pFounder.actor.getName();
                lastName = pFounder.actor.getName();
                families.Add(this);
                addToFamily(pFounder, this);
            }
        }

        public static Family addToFamily(HeraldicInfo pInfo, Family pFamily)
        {
            pInfo.family = pFamily;
            pFamily.members.Add(pInfo);
            return pFamily;
        }
    }
}
