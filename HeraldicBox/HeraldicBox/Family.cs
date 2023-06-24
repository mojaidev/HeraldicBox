using ReflectionUtility;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HeraldicBox
{ 
    class Family
    {
        // ====================================================
        // PATCHES SECTION
        // ====================================================

        public static void killHimself_Postfix(Actor __instance)
        {
            HeraldicComponent component = __instance.GetComponent<HeraldicComponent>();

            if(component == null)
            {
                return;
            }

            if(component.Heraldic.family == null)
            {
                // Why would it be null??
                return;
            }

            component.Heraldic.family.aliveMembers.Remove(component.Heraldic);
        }

        public static bool getName_Prefix(ActorBase __instance, ref string __result)
        {
            ActorData data = Reflection.GetField(typeof(ActorBase), __instance, "data") as ActorData;
            if (string.IsNullOrEmpty(data.name) == false)
            {
                return true;
            }

            HeraldicComponent component = __instance.GetComponent<HeraldicComponent>();

            if (component == null)
            {
                return true;
            }

            HeraldicInfo info = component.Heraldic;

            if (info.family == null)
            {
                return true;
            }

            Race instanceRace = Reflection.GetField(typeof(ActorBase), __instance, "race") as Race;
            data.generateName(__instance.asset, instanceRace);
            data.name = data.name + " " + info.family.lastName;

            if (info.mother != null && info.mother.family != null)
            {
                if (info.mother.family.lastName != info.family.lastName)
                {
                    data.name = data.name + " " + info.mother.family.lastName;
                }
            }

            __result = data.name;
            return false;
        }

        public static void clearTiles_Postfix()
        {
            // This is also a patch for generateNewMap

            families = new List<Family>();
        }

        // ====================================================
        // FAMILY SECTION
        // ====================================================

        public static List<Family> families = new List<Family>();
        public int averageReputation;
        //public Color familyColor = Color.green;
        [NonSerialized] public Sprite shield = Resources.Load<Sprite>("ui/heraldic_banners/2");
        public string familyName, lastName;

        public string publicID; // <-- This is for the save system.

        public List<HeraldicInfo> members = new List<HeraldicInfo>();
        public List<HeraldicInfo> aliveMembers = new List<HeraldicInfo>();

        public void Destroy()
        {
            foreach(HeraldicInfo member in members)
            {
                if(member.actor != null)
                {
                    UnityEngine.Object.Destroy(member.actor.gameObject.GetComponent<HeraldicComponent>());
                }
            }

            families.Remove(this);
        }


        public Family(HeraldicInfo pFounder)
        {
            if(pFounder == null)
            {
                return;
            }

            publicID = Guid.NewGuid().ToString();
            familyName = "The " + pFounder.actor.getName() + "'s Family";
            lastName = pFounder.actor.getName();
            families.Add(this);
            addToFamily(pFounder, this);
        }

        public static Family addToFamily(HeraldicInfo pInfo, Family pFamily)
        {
            pInfo.family = pFamily;
            pFamily.members.Add(pInfo);
            pFamily.aliveMembers.Add(pInfo);
            return pFamily;
        }
    }
}
